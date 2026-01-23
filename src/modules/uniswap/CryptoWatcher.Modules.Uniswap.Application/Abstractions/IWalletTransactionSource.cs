using CryptoWatcher.Modules.Uniswap.Application.Models;

namespace CryptoWatcher.Modules.Uniswap.Application.Abstractions;

public interface IWalletTransactionSource
{
    Task<IReadOnlyCollection<BlockchainTransaction>> GetWalletTransactionsAsync(
        EtherscanTransactionQuery etherscanTransactionQuery,
        CancellationToken ct = default);
}