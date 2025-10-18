namespace CryptoWatcher.Modules.Aave.Application.Models;

public class MarketReserveOutput
{
    public required byte NetworkBaseTokenPriceDecimals { get; init; }
    
    public required IReadOnlyCollection<AggregatedMarketReserveData> AggregatedMarketReserveData { get; init; }
}