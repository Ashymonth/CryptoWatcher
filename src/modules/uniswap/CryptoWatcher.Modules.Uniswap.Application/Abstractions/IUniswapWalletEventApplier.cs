using CryptoWatcher.Modules.Uniswap.Application.Models;
using CryptoWatcher.Modules.Uniswap.Entities;
using CryptoWatcher.Shared.Entities;

namespace CryptoWatcher.Modules.Uniswap.Application.Abstractions;

public interface IUniswapWalletEventApplier
{
    IAsyncEnumerable<WalletEventExtractionResult> ApplyEventsToPositionsAsync(
        UniswapChainConfiguration chainConfiguration,
        UniswapSynchronizationState synchronizationState,
        Wallet wallet,
        CancellationToken ct = default);
}