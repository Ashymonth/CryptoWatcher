using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Morpho.Application.Abstractions;

public interface IMorphoMarketSynchronizer
{
    Task SynchronizeAsync(EvmAddress walletAddress, int chainId, DateTime syncDate, CancellationToken ct = default);
}