using CryptoWatcher.Modules.Morpho.Application.Models;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Morpho.Application.Abstractions;

public interface IMorphoProvider
{
    Task<MorphoMarketPositionData[]> GetUserMarketPositionsAsync(EvmAddress address, int chainId,
        CancellationToken ct = default);
}