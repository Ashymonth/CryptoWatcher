namespace CryptoWatcher.Modules.Uniswap.Infrastructure.Integrations.Etherscan.Contracts.TransactionHistory;

public class EtherscanTransactionHistoryItem
{
    public string Timestamp { get; set; } = null!;

    public string Hash { get; set; } = null!;

    public string FunctionName { get; set; } = null!;

    public string BlockNumber { get; set; } = null!;

    public string To { get; set; } = null!;
}