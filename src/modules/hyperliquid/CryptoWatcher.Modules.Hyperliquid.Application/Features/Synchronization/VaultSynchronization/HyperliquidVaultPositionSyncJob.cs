using CryptoWatcher.Abstractions;
using CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization.Abstractions;
using CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization.Modes;
using CryptoWatcher.Modules.Hyperliquid.Entities;
using CryptoWatcher.Modules.Hyperliquid.Specifications;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization;

public class HyperliquidVaultPositionSyncJob : IHyperliquidVaultPositionSyncJob
{
    private readonly IRepository<HyperliquidVaultPosition> _repository;
    private readonly IRepository<HyperliquidSynchronizationState> _synchronizationStateRepository;
    private readonly IHyperliquidVaultPositionUpdater _positionUpdater;
    private readonly IHyperliquidSnapshotsUpdater _snapshotsUpdater;

    public HyperliquidVaultPositionSyncJob(IRepository<HyperliquidVaultPosition> repository,
        IRepository<HyperliquidSynchronizationState> synchronizationStateRepository,
        IHyperliquidVaultPositionUpdater positionUpdater, IHyperliquidSnapshotsUpdater snapshotsUpdater)
    {
        _repository = repository;
        _synchronizationStateRepository = synchronizationStateRepository;
        _positionUpdater = positionUpdater;
        _snapshotsUpdater = snapshotsUpdater;
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
        return await _synchronizationStateRepository.GetByIdAsync(walletAddress, ct)
               ?? new HyperliquidSynchronizationState(walletAddress);
    }

    private async Task<HyperliquidVaultPosition?> LoadPositionAsync(
        EvmAddress walletAddress, CancellationToken ct)
    {
        return await _repository.FirstOrDefaultAsync(
            new HyperliquidPositionsWithSnapshotsAndCashFlowByWallet(walletAddress), ct);
    }

    private async Task SaveChangesAsync(
        HyperliquidPositionSyncResult syncResult,
        HyperliquidSynchronizationState state,
        CancellationToken ct)
    {
        await _repository.UnitOfWork.BeginTransactionAsync(ct);
        try
        {
            await _repository.BulkMergeAsync([syncResult.Position], ct);

            if (syncResult.LastHyperliquidVaultUpdate is not null)
            {
                state.UpdateLastProcessedEvent(
                    syncResult.LastHyperliquidVaultUpdate!.Timestamp,
                    syncResult.LastHyperliquidVaultUpdate.Hash);

                await _synchronizationStateRepository.BulkMergeAsync([state], ct);
            }

            await _repository.UnitOfWork.CommitTransactionAsync(ct);
        }
        catch
        {
            await _repository.UnitOfWork.RollbackTransactionAsync(ct);
            throw;
        }
    }
}