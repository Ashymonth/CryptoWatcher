namespace AaveClient;

/// <summary>
/// https://aave.com/docs/resources/addresses
/// </summary>
internal static class NetworkRegistry
{
    internal record NetworkInfo(string RpcAddress, string PoolAddress, string ProviderAddress);

    public static readonly Dictionary<AaveNetwork, NetworkInfo> NetworkToRpcUrl = new()
    {
        [AaveNetwork.Sonic] =
            new NetworkInfo("https://rpc.soniclabs.com", "0x9005A69fE088680827f292e8aE885Be4BE1beb2f",
                "0x5C2e738F6E27bCE0F7558051Bf90605dD6176900"),
    };
}