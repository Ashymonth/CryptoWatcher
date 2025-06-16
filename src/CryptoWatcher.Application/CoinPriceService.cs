using CryptoWatcher.Integrations;
using Microsoft.Extensions.Caching.Hybrid;

namespace CryptoWatcher.Application;

public class CoinPriceService
{
    private readonly ICoinPriceProvider _coinPriceProvider;
    private readonly HybridCache _cache;

    public CoinPriceService(ICoinPriceProvider coinPriceProvider, HybridCache cache)
    {
        _coinPriceProvider = coinPriceProvider;
        _cache = cache;
    }

    public async Task<string> GetTokenIdByTokenSymbolAsync(string symbol, CancellationToken ct)
    {
        var cacheKey = string.Format(CacheKeys.TokenId.TokenIdByTokenSymbolTemplate, symbol.ToLower());
        return await _cache.GetOrCreateAsync<string>(cacheKey, async token =>
        {
            throw new InvalidOperationException();
        }, cancellationToken: ct);
    }
    
    public async Task<Dictionary<string, string>> RefreshCacheAsync(CancellationToken ct = default)
    {
        var coinsInfo = await _coinPriceProvider.GetSymbolToIdAsync(ct);

        foreach (var coinInfo in coinsInfo)
        {
            var cacheKey = string.Format(CacheKeys.TokenId.TokenIdByTokenSymbolTemplate, coinInfo.Key);
            await _cache.SetAsync(cacheKey, coinInfo.Value,
                new HybridCacheEntryOptions
                    { Expiration = TimeSpan.FromSeconds(CacheKeys.TokenId.CacheLifetimeInSecond) },
                cancellationToken: ct);
        }

        return coinsInfo;
    }
}