using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Hyperliquid;

public static class HyperliquidWellKnowFields
{
    public static readonly EvmAddress
        HlpVaultAddress = EvmAddress.Create("0xdfc24b077bc1425ad1dea75bcb6f8158e10df303");
    
    public static readonly EvmAddress
        UsdcAddress = EvmAddress.Create("0xaf88d065e77c8cC2239327C5EDb3A432268e5831");
    
    public const string UsdcSymbol = "USDC";
    public const int UsdcPrice = 1;
}