using CryptoWatcher.Abstractions;
using CryptoWatcher.Modules.Hyperliquid.Abstractions;
using CryptoWatcher.Modules.Hyperliquid.Entities;
using CryptoWatcher.Modules.Hyperliquid.Specifications;
using CryptoWatcher.Shared.Entities;

namespace CryptoWatcher.Modules.Hyperliquid.Application.Services;

public interface IHyperliquidPositionsSyncService
{
    /// <summary>
    /// Synchronizes the positions of the given wallet for the specified day.
    /// </summary>
    /// <param name="wallet">The cryptocurrency wallet whose positions are to be synchronized.</param>
    /// <param name="from">The specific day for which positions are being synchronized.</param>
    /// <param name="to"></param>
    /// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous synchronization operation.</returns>
    Task SyncPositionsAsync(Wallet wallet, DateOnly from, DateOnly to, CancellationToken ct = default);
}

/// <summary>
/// <see cref="IHyperliquidPositionsSyncService"/>
/// </summary>
public class HyperliquidPositionsSyncService : IHyperliquidPositionsSyncService
{
    private readonly IHyperliquidProvider _hyperliquidProvider;
    private readonly IRepository<HyperliquidVaultPosition> _repository;
    private readonly TimeProvider _timeProvider;

    public HyperliquidPositionsSyncService(IHyperliquidProvider hyperliquidProvider,
        IRepository<HyperliquidVaultPosition> repository,
        TimeProvider timeProvider)
    {
        _hyperliquidProvider = hyperliquidProvider;
        _repository = repository;

        _timeProvider = timeProvider;
    }

    public async Task SyncPositionsAsync(Wallet wallet, DateOnly from, DateOnly to, CancellationToken ct = default)
    {
        var existingPositionMap =
            (await _repository.ListAsync(new HyperliquidPositionsWithSnapshotsAndCashFlowByWallet(wallet, from, to),
                ct))
            .ToDictionary(position => position.VaultAddress);

        var hyperliquidVaultPositions = await _hyperliquidProvider.GetVaultsPositionsEquityAsync(wallet, ct);
 
        var cashFlowHistory = (await _hyperliquidProvider.GetCashFlowEventsAsync(wallet, from, to, ct))
            .GroupBy(@event => @event.VaultAddress)
            .ToDictionary(events => events.Key, events => events.OrderBy(@event => @event.Date).ToArray());

        var now = _timeProvider.GetUtcNow().UtcDateTime;

        var nowDay = DateOnly.FromDateTime(now);

        await _repository.UnitOfWork.BeginTransactionAsync(ct);

        var result = new List<HyperliquidVaultPosition>(hyperliquidVaultPositions.Length);

        foreach (var (vaultAddress, equity) in hyperliquidVaultPositions)
        {
            try
            {
                var isPositionExist = existingPositionMap.TryGetValue(vaultAddress, out var vaultPosition);
                if (!isPositionExist)
                {
                    vaultPosition = new HyperliquidVaultPosition
                    {
                        InitialBalance = equity,
                        WalletAddress = wallet.Address,
                        VaultAddress = vaultAddress,
                        Wallet = wallet,
                        CreatedAt = nowDay
                    };
                }

                if (equity == 0)
                {
                    vaultPosition!.ClosePosition(nowDay);
                }

                vaultPosition!.AddOrUpdateSnapshot(
                    new HyperliquidVaultPositionSnapshot(wallet, vaultAddress, equity, nowDay));

                if (!cashFlowHistory.TryGetValue(vaultAddress, out var cashFlowEvents))
                {
                    continue;
                }

                foreach (var cashFlowEvent in cashFlowEvents)
                {
                    vaultPosition.AddCashFlowIfNotExists(cashFlowEvent);
                }

                result.Add(vaultPosition);
            }
            catch
            {
                await _repository.UnitOfWork.RollbackTransactionAsync(ct);
                throw;
            }
        }

        await _repository.BulkMergeAsync(result, ct);
        await _repository.UnitOfWork.CommitTransactionAsync(ct);
    }
}