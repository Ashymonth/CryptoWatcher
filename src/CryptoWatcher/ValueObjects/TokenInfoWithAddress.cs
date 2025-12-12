using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Shared.ValueObjects;

/// <summary>
/// Contains metadata and pricing information about a token.
/// </summary>
public record TokenInfoWithAddress : TokenInfo
{
    public TokenInfoWithAddress()
    {
        
    }

    public TokenInfoWithAddress(TokenInfo tokenInfo, EvmAddress address)
    {
        Symbol = tokenInfo.Symbol;
        Amount = tokenInfo.Amount;
        PriceInUsd = tokenInfo.PriceInUsd;
        Address = address;
    }
    
    /// <summary>
    /// Token contract address.
    /// </summary>
    public string Address { get; init; } = null!;
}