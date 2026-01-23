using CryptoWatcher.Modules.Uniswap.Infrastructure.Integrations.Blockscout.Contracts.Shared;
using CryptoWatcher.Modules.Uniswap.Infrastructure.Integrations.Blockscout.Contracts.TransactionHistory;

namespace CryptoWatcher.Modules.Uniswap.Infrastructure.Integrations.Blockscout.Contracts.InternalTransactions;

public class BlockscoutInternalTransactionsItem
{
    public string Type { get; set; } = null!;

    public string Value { get; set; } = null!;

    public BlockscoutSender To { get; set; } = null!;

    public DateTime TimeStamp { get; set; }
}