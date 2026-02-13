using CryptoWatcher.Abstractions;
using CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization.Abstractions;
using CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization.Modes;
using CryptoWatcher.Modules.Hyperliquid.Entities;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization;

public class HyperliquidVaultPositionSyncJob : IHyperliquidVaultPositionSyncJob
{
    private readonly IHyperliquidVaultPositionUpdater _positionUpdater;
    private readonly IHyperliquidSnapshotsUpdater _snapshotsUpdater;
    private readonly IHyperliquidSyncStateRepository _synchronizationStateRepository;
    private readonly IHyperliquidVaultPositionRepository _positionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public HyperliquidVaultPositionSyncJob(IHyperliquidVaultPositionUpdater positionUpdater,
        IHyperliquidSnapshotsUpdater snapshotsUpdater, IHyperliquidSyncStateRepository synchronizationStateRepository,
        IHyperliquidVaultPositionRepository positionRepository, IUnitOfWork unitOfWork)
    {
        _positionUpdater = positionUpdater;
        _snapshotsUpdater = snapshotsUpdater;
        _synchronizationStateRepository = synchronizationStateRepository;
        _positionRepository = positionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task SyncPositionAsync(EvmAddress walletAddress,
        DateTime snapshotDate,
        CancellationToken ct = default)
    {
        var state = await LoadOrCreateStateAsync(walletAddress, ct);
        var position = await LoadPositionAsync(walletAddress, ct);
        
        var updatedPosition = await _positionUpdater.UpdatePositionAsync(position, walletAddress, state, ct);

        if (updatedPosition is null)
        {
            return;
        }

        await _snapshotsUpdater.TakeVaultBalanceSnapshotAsync(updatedPosition.Position, snapshotDate, ct);

        await SaveChangesAsync(updatedPosition, state, ct);
    }

    private async Task<HyperliquidSynchronizationState> LoadOrCreateStateAsync(
        EvmAddress walletAddress, CancellationToken ct)
    {
        return await _synchronizationStateRepository.GetByWalletAsync(walletAddress, ct)
               ?? new HyperliquidSynchronizationState(walletAddress);
    }

    private async Task<HyperliquidVaultPosition?> LoadPositionAsync(
        EvmAddress walletAddress, CancellationToken ct)
    {
        return await _positionRepository.GetByWalletAsync(walletAddress, ct);
    }

    private async Task SaveChangesAsync(HyperliquidPositionSyncResult syncResult, HyperliquidSynchronizationState state,
        CancellationToken ct)
    {
        await _unitOfWork.ExecuteAsync(async token =>
        {
            await _positionRepository.AddOrUpdateAsync(syncResult.Position, token);

            if (syncResult.LastHyperliquidVaultUpdate is not null)
            {
                state.UpdateLastProcessedEvent(
                    syncResult.LastHyperliquidVaultUpdate!.Timestamp,
                    syncResult.LastHyperliquidVaultUpdate.Hash);

                await _synchronizationStateRepository.AddOrUpdateAsync(state, token);
            }
        }, ct);
    }
}