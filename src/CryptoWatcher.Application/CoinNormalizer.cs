namespace CryptoWatcher.Application;

public class CoinNormalizer
{
    private static readonly Dictionary<string, string> SymbolToNormalizedSymbol = new()
    {
        ["USD₮0"] = "usdt"
    };

    private static readonly Dictionary<string, string> AddressToNormalizedAddress = new()
    {
        ["0x0000000000000000000000000000000000000000"] = "0xc02aaa39b223fe8d0a0e5c4f27ead9083c756cc2"
    };

    public string NormalizeSymbol(string symbol)
    {
        return SymbolToNormalizedSymbol.GetValueOrDefault(symbol, symbol);
    }

    public string NormalizeAddress(string address)
    {
        return AddressToNormalizedAddress.GetValueOrDefault(address) ?? address;
    }
}