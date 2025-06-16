using System.Net.Http.Json;
using System.Text.Json;
using CoinGeckoClient.CoinsList;
using CoinGeckoClient.Price;

namespace CoinGeckoClient;

public interface ICoinGeckoApiClient
{
    Task<decimal> GetTokenPriceInUsdAsync(GetTokenPriceInUsdRequest request, CancellationToken ct = default);

    Task<CoinInfo[]> GetCoinsListAsync(CancellationToken ct = default);
}

public class CoinGeckoApiClient : ICoinGeckoApiClient
{
    private static readonly Dictionary<string, string> Symbol2Id = new()
    {
        ["USDC"] = "usd-coin",
        ["wrsETH"] = "WETH",
        ["weth"] = "WETH",
        ["wstETH"] = "WETH",
        ["ETH"] = "WETH",
        ["USD₮0"] = "usd-coin",
    };

    private readonly HttpClient _client;

    public CoinGeckoApiClient(HttpClient client)
    {
        _client = client;
    }
    
    public async Task<CoinInfo[]> GetCoinsListAsync(CancellationToken ct)
    {
        var result = await _client.GetFromJsonAsync<CoinInfo[]>("/api/v3/coins/list?include_platform=true", ct);

        return result!;
    }

    public async Task<decimal> GetTokenPriceInUsdAsync(GetTokenPriceInUsdRequest request, CancellationToken ct)
    {
        var id = Symbol2Id.GetValueOrDefault(request.Symbol) ?? request.Symbol;
        var url = $"api/v3/simple/price?ids={id}&vs_currencies=usd";

        var result = await _client.GetFromJsonAsync<Dictionary<string, TokenPriceInfo>>(url, ct);
        
        return result![id.ToLower()].Usd;
    }
}