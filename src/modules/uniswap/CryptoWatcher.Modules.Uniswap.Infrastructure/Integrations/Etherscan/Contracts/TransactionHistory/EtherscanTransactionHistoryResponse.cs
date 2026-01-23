namespace CryptoWatcher.Modules.Uniswap.Infrastructure.Integrations.Etherscan.Contracts.TransactionHistory;

public class EtherscanTransactionHistoryResponse
{
    public EtherscanTransactionHistoryItem[] Result { get; init; } = [];
}