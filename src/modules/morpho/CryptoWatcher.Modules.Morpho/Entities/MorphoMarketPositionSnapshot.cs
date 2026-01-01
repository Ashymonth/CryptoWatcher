using CryptoWatcher.Modules.Morpho.Abstractions;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Morpho.Entities;

public class MorphoMarketPositionSnapshot : IMarketPositionSnapshot
{
    public MorphoMarketPositionSnapshot(
        Guid morphoMarketPositionId,
        DateOnly day, 
        CryptoTokenStatistic loadToken,
        CryptoTokenStatistic collateralToken,
        double healthFactor)
    {
        MorphoMarketPositionId = morphoMarketPositionId;
        Day = day;
        LoadToken = loadToken;
        CollateralToken = collateralToken;
        HealthFactor = healthFactor;
    }

    public DateOnly Day { get; private set; }

    public double HealthFactor { get; private set; }

    public CryptoTokenStatistic LoadToken { get; private set; }

    public CryptoTokenStatistic CollateralToken { get; private set; }

    public Guid MorphoMarketPositionId { get; private set; }

    public void UpdateSnapshot(CryptoTokenStatistic loadToken, CryptoTokenStatistic collateralToken,
        double healthFactor)
    {
        LoadToken = loadToken;
        CollateralToken = collateralToken;
        HealthFactor = healthFactor;
    }
}