using CryptoWatcher.Modules.Uniswap.Application.Models;
using Riok.Mapperly.Abstractions;

namespace CryptoWatcher.Modules.Uniswap.Infrastructure.Integrations.Etherscan.Contracts.TransactionHistory;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class EtherscanTransactionHistoryMapper
{
    public static partial EtherscanTransactionHistoryQueryParams MapQuery(this EtherscanTransactionQuery query);

    [MapProperty(nameof(BlockchainTransaction.Timestamp), nameof(EtherscanTransactionHistoryItem.Timestamp),
        Use = nameof(MapDateTime))]
    public static partial BlockchainTransaction MapToBlockchainTransaction(this EtherscanTransactionHistoryItem item);

    private static DateTime MapDateTime(this string timestamp) =>
        DateTimeOffset.FromUnixTimeSeconds(long.Parse(timestamp)).UtcDateTime;
}