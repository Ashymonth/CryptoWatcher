using CryptoWatcher.Modules.Uniswap.Application.Services.Synchronization.PositionsEventsSynchronization.UniswapV3.Models.PositionEvents;
using CryptoWatcher.Modules.Uniswap.Entities;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Uniswap.Application.Abstractions;

public interface IUniswapTransactionEventSource
{
    Task<UniswapPositionEvent?> GetUniswapEventAsync(UniswapChainConfiguration chain, TransactionHash hash,
        CancellationToken ct = default);
}