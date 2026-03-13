using CryptoWatcher.Modules.Contracts.Messages;

namespace CryptoWatcher.Modules.Uniswap.Application.Abstractions;

public interface IWalletTransactionConsumer
{
    Task ConsumeTransactionsAsync(IEnumerable<BlockchainTransaction> blockchainTransaction,
        CancellationToken ct = default);
}