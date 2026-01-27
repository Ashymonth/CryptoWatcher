using CryptoWatcher.Modules.Uniswap.Entities;
using CryptoWatcher.Shared.Entities;

namespace CryptoWatcher.Modules.Uniswap.Application.Abstractions;

public interface IPositionPriceSynchronizer
{
    IAsyncEnumerable<UniswapLiquidityPosition> SynchronizeAsync(UniswapChainConfiguration chain,
        Wallet wallet,
        IReadOnlyCollection<UniswapLiquidityPosition> dbPositions,
        DateOnly day,
        CancellationToken ct = default);
}