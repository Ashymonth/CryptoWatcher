using AaveClient;
using CryptoWatcher.Infrastructure.Configs;

namespace CryptoWatcher.Infrastructure.Extensions;

public static class AaveNetworkTypeExtensions
{
    public static string GetMainnetAddress(this AaveNetworkType aaveNetworkType, AaveConfig _aaveConfig)
    {
        return aaveNetworkType switch
        {
            AaveNetworkType.Celo => _aaveConfig.CelloMainnetAddress,
            AaveNetworkType.Sonic => _aaveConfig.SonicMainnetAddress,
            AaveNetworkType.Avalanche => _aaveConfig.AvalancheMainnetAddress,
            _ => throw new ArgumentOutOfRangeException(nameof(aaveNetworkType), aaveNetworkType, null)
        };
    }
}