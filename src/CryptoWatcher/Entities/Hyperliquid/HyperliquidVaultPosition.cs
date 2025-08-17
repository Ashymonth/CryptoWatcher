namespace CryptoWatcher.Entities.Hyperliquid;

public class HyperliquidVaultPosition
{
    public string VaultAddress { get; init; } = null!;
    
    /// <summary>
    /// Represents the wallet address associated with the liquidity pool position.
    /// </summary>
    /// <remarks>
    /// This property holds the blockchain wallet address linked to the liquidity pool position.
    /// It is used to identify the owner of the position and manage the related account details.
    /// </remarks>
    public string WalletAddress { get; init; } = null!;

    /// <summary>
    /// Represents the wallet associated with a liquidity pool position.
    /// </summary>
    /// <remarks>
    /// This property identifies the wallet that holds ownership of the liquidity pool position.
    /// It includes the wallet's unique identifier and blockchain address for managing assets.
    /// </remarks>
    public Wallet Wallet { get; init; } = null!;

    public List<HyperliquidVaultEvent> VaultEvents { get; init; } = [];

    public List<HyperliquidVaultPositionSnapshot> PositionSnapshots { get; init; } = [];
}