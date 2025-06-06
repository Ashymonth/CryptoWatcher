namespace CryptoWatcher.Models;

/// <summary>
/// Contains metadata and pricing information about a token.
/// </summary>
public record TokenInfo
{
    /// <summary>
    /// Token contract address.
    /// </summary>
    public string Address { get; set; } = null!;

    /// <summary>
    /// Token symbol (e.g., "ETH", "USDC").
    /// </summary>
    public string Symbol { get; set; } = null!;

    /// <summary>
    /// Token amount.
    /// </summary>
    public decimal Amount { get; set; }
    
    /// <summary>
    /// Token price in USD.
    /// </summary>
    public decimal PriceInUsd { get; set; }
}