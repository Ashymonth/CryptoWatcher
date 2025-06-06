namespace CryptoWatcher.Entities;

public class Network
{
    /// <summary>
    /// The network name.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// RPC URL of the target blockchain network.
    /// </summary>
    public string RpcUrl { get; set; } = null!;

    /// <summary>
    /// Address of the Uniswap V3 NonfungiblePositionManager contract.
    /// </summary>
    public string NftManagerAddress { get; set; } = null!;

    /// <summary>
    /// Address of the Uniswap V3 Pool Factory contract.
    /// </summary>
    public string PoolFactoryAddress { get; set; } = null!;
 
    /// <summary>
    /// Address of the Uniswap V3 Pool Factory contract.
    /// </summary>
    public string MultiCallAddress { get; set; } = null!;
}