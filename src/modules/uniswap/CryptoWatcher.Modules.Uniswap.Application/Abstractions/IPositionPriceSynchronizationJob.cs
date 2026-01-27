namespace CryptoWatcher.Modules.Uniswap.Application.Abstractions;

public interface IPositionPriceSynchronizationJob
{
    Task SynchronizeAsync(CancellationToken ct = default);
}