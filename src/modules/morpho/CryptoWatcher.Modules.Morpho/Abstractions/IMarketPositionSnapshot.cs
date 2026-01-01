using CryptoWatcher.Abstractions.PositionSnapshots;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Morpho.Abstractions;

public interface IMarketPositionSnapshot : IPositionSnapshot
{
    CryptoTokenStatistic LoadToken { get; }
    
    CryptoTokenStatistic CollateralToken { get; }
}