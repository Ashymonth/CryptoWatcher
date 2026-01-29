namespace CryptoWatcher.Modules.Uniswap.Application.Abstractions;

public interface IUniswapWalletPositionsSyncJob
{
    Task SynchronizeAsync(CancellationToken ct = default);
}