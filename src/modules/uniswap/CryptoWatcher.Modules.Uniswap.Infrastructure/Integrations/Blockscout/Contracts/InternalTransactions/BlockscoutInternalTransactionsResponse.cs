namespace CryptoWatcher.Modules.Uniswap.Infrastructure.Integrations.Blockscout.Contracts.InternalTransactions;

public class BlockscoutInternalTransactionsResponse
{
    public List<BlockscoutInternalTransactionsItem> Items { get; init; } = [];
}