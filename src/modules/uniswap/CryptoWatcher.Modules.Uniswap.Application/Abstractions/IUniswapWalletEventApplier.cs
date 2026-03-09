using CryptoWatcher.Modules.Uniswap.Application.Models;
using CryptoWatcher.Modules.Uniswap.Entities;

namespace CryptoWatcher.Modules.Uniswap.Application.Abstractions;

public interface IUniswapWalletEventApplier
{
    IAsyncEnumerable<UniswapLiquidityPosition[]> ApplyEventsToPositionsAsync(
        UniswapChainConfiguration chainConfiguration,
        IEnumerable<BlockchainTransaction> transaction,
        CancellationToken ct = default);
}