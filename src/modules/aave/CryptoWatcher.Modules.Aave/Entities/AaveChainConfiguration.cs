using CryptoWatcher.Modules.Aave.ValueObjects;

namespace CryptoWatcher.Modules.Aave.Entities;

public class AaveChainConfiguration : BaseChainConfiguration
{
    public AaveAddresses SmartContractAddresses { get; init; } = null!;
}