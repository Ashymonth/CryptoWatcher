using CryptoWatcher.Modules.Uniswap.Application.Models;
using CryptoWatcher.Modules.Uniswap.Entities;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Uniswap.Application.Abstractions;

public interface IUniswapWalletTransactionScanner
{
    IAsyncEnumerable<WalletTransactionScanResult> ScanWalletTransactionsAsync(UniswapChainConfiguration chain,
        UniswapSynchronizationState synchronizationState, EvmAddress wallet,
        CancellationToken ct = default);
}