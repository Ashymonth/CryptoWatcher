using Refit;

namespace CryptoWatcher.Modules.Uniswap.Infrastructure.Integrations.Blockscout.Contracts.TransactionHistory;

public record BlockscoutTransactionHistoryQueryParams
{
    public required int Index { get; set; }

    public required string Hash { get; set; } = null!;

    [AliasAs("block_number")] public required long BlockNumber { get; set; }

    [AliasAs("items_count")] public required int ItemsCount { get; set; }
}