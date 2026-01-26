using CryptoWatcher.Modules.Uniswap.Application.Models;
using CryptoWatcher.Modules.Uniswap.Application.Services.PoisitionEventsSync.UniswapV3.Models.Operations;
using CryptoWatcher.Modules.Uniswap.Entities;

namespace CryptoWatcher.Modules.Uniswap.Application.Abstractions;

public interface IUniswapTransactionEnricher
{
    Task<UniswapEvent?> TryEnrichAsync(UniswapChainConfiguration chainConfiguration, BlockchainTransaction transaction,
        CancellationToken ct = default);
}