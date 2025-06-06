namespace CryptoWatcher.Entities;

/// <summary>
/// Represents a blockchain network and its related configuration data.
/// </summary>
/// <remarks>
/// The Network class contains details about a blockchain network, including the network's name,
/// RPC URL for connecting to its nodes, core contract addresses for NFTs and pools, and a collection
/// of pool history records associated with the network.
/// </remarks>
public class Network
{
    /// <summary>
    /// The name of the blockchain network.
    /// </summary>
    /// <remarks>
    /// This property serves as a unique identifier for the network and is used in various configurations
    /// and operations, such as database key constraints and referencing associated data.
    /// </remarks>
    public string Name { get; init; } = null!;

    /// <summary>
    /// The RPC (Remote Procedure Call) URL for connecting to the blockchain network's nodes.
    /// </summary>
    /// <remarks>
    /// This property provides the endpoint through which interactions with the blockchain network are performed,
    /// such as fetching data, submitting transactions, or invoking smart contract functions.
    /// It is an essential configuration for network communication in decentralized application setups.
    /// </remarks>
    public string RpcUrl { get; init; } = null!;

    /// <summary>
    /// The contract address for the NFT Manager on the blockchain network.
    /// </summary>
    /// <remarks>
    /// This property stores the address of the NFT Manager smart contract, which is responsible
    /// for managing Non-Fungible Tokens (NFTs) on the network. It is critical for interacting
    /// with NFT-related functionalities, such as minting, transferring, and updating tokens.
    /// </remarks>
    public string NftManagerAddress { get; init; } = null!;

    /// <summary>
    /// The address of the pool factory contract.
    /// </summary>
    /// <remarks>
    /// This property represents the blockchain address of the contract responsible for creating
    /// and managing liquidity pools in the decentralized finance system. It is a key integration point
    /// for interacting with pools and executing related operations.
    /// </remarks>
    public string PoolFactoryAddress { get; init; } = null!;

    /// <summary>
    /// The address of the MultiCall contract associated with the blockchain network.
    /// </summary>
    /// <remarks>
    /// This property is used to facilitate batched execution of multiple read-only contract calls,
    /// optimizing performance by reducing the number of network requests.
    /// </remarks>
    public string MultiCallAddress { get; init; } = null!;

    /// <summary>
    /// A collection of historical data records related to liquidity pools within the blockchain network.
    /// </summary>
    /// <remarks>
    /// This property contains a list of <see cref="LiquidityPoolPositionSnapshot"/> objects, representing daily snapshots
    /// of pool states, including token amounts, fees, APR, and activity status. The data is associated
    /// with the specific blockchain network and can be used for analytics and historical performance tracking.
    /// </remarks>
    public List<LiquidityPoolPosition> LiquidityPoolPositions { get; init; } = [];
}