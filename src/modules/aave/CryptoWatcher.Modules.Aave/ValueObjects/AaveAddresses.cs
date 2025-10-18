using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Aave.ValueObjects;

public class AaveAddresses
{
    public required EvmAddress UiPoolDataProviderAddress { get; init; }

    public required EvmAddress PoolAddressesProviderAddress { get; init; }
}