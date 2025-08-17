using CryptoWatcher.Abstractions;
using CryptoWatcher.Data;
using CryptoWatcher.Entities.Hyperliquid;
using CryptoWatcher.Integrations;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using TickerQ.Utilities.Base;

namespace CryptoWatcher.Host.Jobs;

public class SyncHyperliquidPositions
{
    private readonly CryptoWatcherDbContext _context;
    private readonly IHyperliquidProvider _hyperliquidProvider;
    private readonly IRepository<HyperliquidVaultPosition> _repository;

    public SyncHyperliquidPositions(CryptoWatcherDbContext context, IHyperliquidProvider hyperliquidProvider,
        IRepository<HyperliquidVaultPosition> repository)
    {
        _context = context;
        _hyperliquidProvider = hyperliquidProvider;
        _repository = repository;
    }

    [TickerFunction(nameof(SyncHyperliquidPositionsAsync), "0 */1 * * *")]
    public async Task SyncHyperliquidPositionsAsync(CancellationToken ct)
    {
        var wallets = await _context.Wallets.ToArrayAsync(ct);

        var now = DateTime.Now;
        foreach (var wallet in wallets)
        {
            using var tr = _repository.UnitOfWork.BeginTransactionAsync(ct);
            var fundingHistory = await _hyperliquidProvider.GetVaultsFundingHistory(wallet, ct);

            var vaultPositions = await _hyperliquidProvider.GetVaultsPositionsEquityAsync(wallet, ct);

            var vaults = fundingHistory.Select(@event => @event.VaultAddress)
                .Distinct()
                .Select(vaultAddress => new HyperliquidVaultPosition
                {
                    WalletAddress = wallet.Address,
                    VaultAddress = vaultAddress
                })
                .ToArray();

            var vaultEvents = fundingHistory.Select(@event =>
                    new HyperliquidVaultEvent
                    {
                        EventType = @event.EventType,
                        VaultAddress = @event.VaultAddress,
                        Usd = @event.Usd,
                        Date = @event.Date,
                        WalletAddress = wallet.Address,
                    })
                .ToList();

            await _context.BulkInsertOrUpdateAsync(vaultEvents, config =>
            {
                config.UpdateByProperties =
                [
                    nameof(HyperliquidVaultEvent.WalletAddress), nameof(HyperliquidVaultEvent.VaultAddress),
                    nameof(HyperliquidVaultEvent.Date)
                ];
            }, cancellationToken: ct);

            var positionSnapshots = vaultPositions.Select(tuple => new HyperliquidVaultPositionSnapshot
                {
                    Balance = tuple.Equity,
                    Day = DateOnly.FromDateTime(now),
                    WalletAddress = wallet.Address,
                    VaultAddress = tuple.VaultAddress,
                })
                .ToList();

            await _context.BulkInsertOrUpdateAsync(positionSnapshots, config =>
            {
                config.UpdateByProperties =
                [
                    nameof(HyperliquidVaultPositionSnapshot.WalletAddress),
                    nameof(HyperliquidVaultPositionSnapshot.VaultAddress), nameof(HyperliquidVaultPositionSnapshot.Day)
                ];
            }, cancellationToken: ct);


            await _repository.BulkMergeAsync(vaults, ct);

            await _repository.UnitOfWork.SaveChangesAsync(ct);
        }
    }
}