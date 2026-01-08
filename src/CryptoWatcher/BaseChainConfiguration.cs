namespace CryptoWatcher;

public abstract class BaseChainConfiguration
{
    /// <summary>
    /// Gets the unique identifier or name of the blockchain chain where the Uniswap protocol is deployed.
    /// </summary>
    /// <remarks>
    /// This property is used to distinguish between different Uniswap chain configurations
    /// and serves as a reference in various operations and database mappings.
    /// </remarks>
    public required string Name { get; init; } = null!;

    /// <summary>
    /// Gets the URL of the RPC (Remote Procedure Call) endpoint for the blockchain network.
    /// </summary>
    /// <remarks>
    /// This property specifies the RPC endpoint used to interact with the blockchain network
    /// for executing transactions, querying data, and other interactions required by the Uniswap protocol.
    /// </remarks>
    public required Uri RpcUrl { get; init; } = null!;
    
    /// <summary>
    /// Gets the authorization token for the RPC endpoint (e.g., API key for Infura, RPC).
    /// </summary>
    /// <remarks>
    /// Stored separately for security; can be rotated independently. Do not log or expose in APIs.
    /// </remarks>
    public string? RpcAuthToken { get; init; } // store later with encryption

    /// <summary>
    /// Gets a composed URL that includes the RPC base URL and the optional authentication token.
    /// </summary>
    /// <remarks>
    /// This property concatenates the base RPC URL with the authentication token, if provided.
    /// It is used to authenticate requests to the blockchain RPC endpoint for interacting with
    /// the Uniswap protocol on a specific chain.
    /// </remarks>
    public Uri RpcUrlWithAuthToken => RpcAuthToken is not null ? new Uri($"{RpcUrl}/{RpcAuthToken}") : RpcUrl;
}