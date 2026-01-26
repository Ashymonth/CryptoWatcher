using CryptoWatcher.Modules.Uniswap.Application.UniswapV3.Models.Operations;
using CryptoWatcher.Modules.Uniswap.Entities;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Uniswap.Application.Abstractions;

public interface IUniswapTransactionEventSource
{
    Task<UniswapEvent?> GetUniswapEventAsync(UniswapChainConfiguration chain, TransactionHash hash,
        CancellationToken ct = default);
}