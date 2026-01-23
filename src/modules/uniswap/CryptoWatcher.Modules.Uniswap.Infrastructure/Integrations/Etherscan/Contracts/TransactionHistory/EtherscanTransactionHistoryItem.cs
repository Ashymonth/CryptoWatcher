namespace CryptoWatcher.Modules.Uniswap.Infrastructure.Integrations.Etherscan.Contracts.TransactionHistory;

public class EtherscanTransactionHistoryItem
{
    public long Timestamp { get; set; }

    public string Hash { get; set; } = null!;

    public string FunctionName { get; set; } = null!;

    public string To { get; set; } = null!;
}