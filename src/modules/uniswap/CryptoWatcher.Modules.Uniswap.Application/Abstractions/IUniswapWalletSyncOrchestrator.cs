namespace CryptoWatcher.Modules.Uniswap.Application.Abstractions;

public interface IUniswapWalletSyncOrchestrator
{
    Task SynchronizeAsync(CancellationToken ct = default);
}