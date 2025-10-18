using CryptoWatcher.Modules.Aave.Infrastructure.Client;

namespace CryptoWatcher.Modules.Aave.Infrastructure.Extensions;

public static class AaveNetworkTypeExtensions
{
    public static string GetMainnetAddress(this AaveNetworkType aaveNetworkType, AaveConfig aaveConfig)
    {
        return aaveNetworkType switch
        {
            AaveNetworkType.Celo => aaveConfig.CelloMainnetAddress,
            AaveNetworkType.Sonic => aaveConfig.SonicMainnetAddress,
            AaveNetworkType.Avalanche => aaveConfig.AvalancheMainnetAddress,
            _ => throw new ArgumentOutOfRangeException(nameof(aaveNetworkType), aaveNetworkType, null)
        };
    }
}