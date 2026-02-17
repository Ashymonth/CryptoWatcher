using CryptoWatcher.Exceptions;
using CryptoWatcher.Modules.Aave.Application.Abstractions;
using CryptoWatcher.Modules.Aave.Application.Models;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Aave.Application.Services;

public class AaveHealthFactorCalculator : IAaveHealthFactorCalculator
{
    public double CalculateHealthFactor(IReadOnlyCollection<CalculatableAaveLendingPosition> userPositions,
        Dictionary<EvmAddress, AggregatedMarketReserveData> marketReserveOutput)
    {
        var collateral = 0m;
        var debt = 0m;

        foreach (var lendingPosition in userPositions)
        {
            if (!marketReserveOutput.TryGetValue(lendingPosition.TokenAddress, out var marketData))
            {
                throw new DomainException($"Market data not found for token: {lendingPosition.TokenAddress}");
            }

            switch (lendingPosition)
            {
                case SuppliedAaveLendingPosition { IsCollateral: true } suppliedPosition:
                {
                    var liquidationThresholdFraction = marketData.ReserveLiquidationThreshold / 10000m;
                    collateral += suppliedPosition.CalculatePositionScaleInToken()
                                  * suppliedPosition.TokenPriceInUsd
                                  * liquidationThresholdFraction;
                    break;
                }
                case BorrowedAaveLendingPosition borrowedPosition:
                {
                    debt += borrowedPosition.CalculatePositionScaleInToken()
                            * borrowedPosition.TokenPriceInUsd;
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(userPositions));
            }
        }

        if (debt == 0)
        {
            return double.MaxValue;
        }

        return (double)(collateral / debt);
    }
}