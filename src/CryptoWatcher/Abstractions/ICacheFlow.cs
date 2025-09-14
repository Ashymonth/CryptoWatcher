using CryptoWatcher.Shared.ValueObjects;

namespace CryptoWatcher.Abstractions;

public interface ICacheFlow
{
    public DateTime Date { get; init; }

    public CacheFlowEvent Event { get; init; }
}

public interface IUsdCacheFlow : ICacheFlow
{
    public decimal Usd { get; init; }
}

public interface ITokenCacheFlow : ICacheFlow
{
    public TokenInfo Token { get; init; }
}

public enum CacheFlowEvent
{
    Deposit = 1,
    Withdraw = 2
}