using Riok.Mapperly.Abstractions;
using BlockscoutNextPageParams = CryptoWatcher.Modules.Uniswap.Application.Models.BlockscoutNextPageParams;

namespace CryptoWatcher.Modules.Uniswap.Infrastructure.Integrations.Blockscout.Contracts.TransactionHistory;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class BlockscoutTransactionHistoryMapper
{
    public static partial BlockscoutTransactionHistoryQueryParams MapToQueryParams(
        this BlockscoutNextPageParams pageParams);
}