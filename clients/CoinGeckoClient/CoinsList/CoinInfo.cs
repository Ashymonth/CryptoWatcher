namespace CoinGeckoClient.CoinsList;

public record CoinInfo(string Id, string Symbol, string Name, Dictionary<string,string> Platforms);