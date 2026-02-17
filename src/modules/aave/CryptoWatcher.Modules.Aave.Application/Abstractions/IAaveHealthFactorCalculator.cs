using CryptoWatcher.Modules.Aave.Application.Models;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Aave.Application.Abstractions;

public interface IAaveHealthFactorCalculator
{
    double CalculateHealthFactor(IReadOnlyCollection<CalculatableAaveLendingPosition> userPositions,
        Dictionary<EvmAddress, AggregatedMarketReserveData> marketReserveOutput);
}