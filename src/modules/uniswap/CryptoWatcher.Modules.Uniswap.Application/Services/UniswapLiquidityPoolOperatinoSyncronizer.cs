using CryptoWatcher.Modules.Uniswap.Application.Abstractions;
using CryptoWatcher.Modules.Uniswap.Entities;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Uniswap.Application.Services;

public class UniswapLiquidityPoolOperatinoSyncronizer
{
    private readonly IWalletTransactionSource _transactionSource;

    public UniswapLiquidityPoolOperatinoSyncronizer(IWalletTransactionSource transactionSource)
    {
        _transactionSource = transactionSource;
    }

    public async Task SyncWalletAsync(UniswapChainConfiguration chainConfiguration, EvmAddress walletAddress,
        CancellationToken ct = default)
    {
    }
}