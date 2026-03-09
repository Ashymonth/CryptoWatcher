using CryptoWatcher.Modules.Uniswap.Application.Models;

namespace CryptoWatcher.Modules.Uniswap.Application.Abstractions;

public interface IWalletTransactionConsumer
{
    IAsyncEnumerable<BlockchainTransaction[]> ConsumeTransactionsAsync(
        IEnumerable<BlockchainTransaction> blockchainTransaction,
        CancellationToken ct = default);
}