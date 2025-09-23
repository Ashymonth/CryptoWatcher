namespace AaveClient;

/// <summary>
/// https://aave.com/docs/resources/addresses
/// </summary>
public static class AaveRegistry
{
    public class SmartContractAddresses
    {
        public required string UiPoolDataProviderAddress { get; init; }

        public required string PoolAddressesProviderAddress { get; init; }
    }
    
    public static readonly Dictionary<AaveNetworkType, SmartContractAddresses> NetworkToRpcUrl = new()
    {
        [AaveNetworkType.Sonic] = new SmartContractAddresses
        {
            UiPoolDataProviderAddress = "0x9005A69fE088680827f292e8aE885Be4BE1beb2f",
            PoolAddressesProviderAddress = "0x5C2e738F6E27bCE0F7558051Bf90605dD6176900"
        },
        [AaveNetworkType.Celo] = new SmartContractAddresses
        {
            UiPoolDataProviderAddress = "0xf07fFd12b119b921C4a2ce8d4A13C5d1E3000d6e",
            PoolAddressesProviderAddress = "0x9F7Cf9417D5251C59fE94fB9147feEe1aAd9Cea5"
        },
        [AaveNetworkType.Avalanche] = new SmartContractAddresses
        {
            UiPoolDataProviderAddress = "0x50B4a66bF4D41e6252540eA7427D7A933Bc3c088",
            PoolAddressesProviderAddress = "0xa97684ead0e402dC232d5A977953DF7ECBaB3CDb"
        }
    };
}