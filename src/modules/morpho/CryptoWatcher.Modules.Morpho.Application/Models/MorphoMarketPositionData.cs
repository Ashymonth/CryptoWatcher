using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Morpho.Application.Models;

public record MorphoMarketPositionData(
    Guid MarketId,
    CryptoToken LoanToken,
    CryptoToken CollateralToken,
    double HealthFactor);