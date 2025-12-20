using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Abstractions.CacheFlows;

public interface ITokenPairCashFlow : ICashFlow
{
    CryptoTokenStatistic Token0 { get; set; }
    
    CryptoTokenStatistic Token1 { get; set; }
}