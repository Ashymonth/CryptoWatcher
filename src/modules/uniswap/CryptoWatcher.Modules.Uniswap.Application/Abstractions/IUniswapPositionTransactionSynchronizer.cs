using CryptoWatcher.Modules.Uniswap.Entities;
using CryptoWatcher.Shared.Entities;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Uniswap.Application.Abstractions;

public interface IUniswapPositionTransactionSynchronizer
{
    Task SynchronizeEventFromTransactionAsync(UniswapChainConfiguration chain,
        Wallet wallet,
        TransactionHash transactionHash,
        CancellationToken ct = default);
}