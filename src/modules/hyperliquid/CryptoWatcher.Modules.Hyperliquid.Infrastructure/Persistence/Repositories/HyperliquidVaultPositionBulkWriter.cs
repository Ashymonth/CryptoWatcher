using CryptoWatcher.Modules.Hyperliquid.Entities;
using CryptoWatcher.Modules.Infrastructure.Shared.Persistence;
using Z.BulkOperations;

namespace CryptoWatcher.Modules.Hyperliquid.Infrastructure.Persistence.Repositories;

public class HyperliquidVaultPositionBulkWriter : BulkWriter<HyperliquidDbContext>
{
    public HyperliquidVaultPositionBulkWriter(HyperliquidDbContext db) : base(db)
    {
    }

    public async ValueTask MergeAsync(
        IReadOnlyList<HyperliquidVaultPosition> aggregates,
        CancellationToken ct)
    {
        await base.MergeAsync(aggregates, op =>
        {
            op.IncludeGraph = true;
            op.IncludeGraphOperationBuilder = Configure;
        }, ct);
    }

    private void Configure(BulkOperation bulkOperation)
    {
        switch (bulkOperation)
        {
            case BulkOperation<HyperliquidVaultPosition> positionOperation:
                positionOperation.ColumnPrimaryKeyExpression = position =>
                    new { position.VaultAddress, position.WalletAddress };
                break;
            case BulkOperation<HyperliquidVaultPositionSnapshot> positionOperation:
                positionOperation.ColumnPrimaryKeyExpression = position =>
                    new { position.VaultAddress, position.WalletAddress, position.Day };
                break;
            case BulkOperation<HyperliquidPositionCashFlow> positionOperation:
                positionOperation.ColumnPrimaryKeyExpression = @event => new
                {
                    @event.VaultAddress, @event.WalletAddress, @event.Date
                };
                break;
            case BulkOperation<HyperliquidVaultPeriod> positionOperation:
                positionOperation.ColumnPrimaryKeyExpression = @event => @event.Id;
                break;
        }
    }
}