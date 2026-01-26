using CryptoWatcher.Modules.Uniswap.Application.Services.PoisitionEventsSync.UniswapV3.Models.Operations;
using CryptoWatcher.Modules.Uniswap.Entities;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Uniswap.Application.Abstractions;

public interface IUniswapLiquidityPositionEventReducer
{
    Task<UniswapLiquidityPosition[]> ApplyEventsAsync(UniswapChainConfiguration chainConfiguration,
        EvmAddress walletAddress,
        IReadOnlyCollection<UniswapEvent> uniswapEvents,
        IReadOnlyCollection<UniswapLiquidityPosition> currentPositions,
        CancellationToken ct = default);
}