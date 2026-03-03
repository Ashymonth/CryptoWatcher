using CryptoWatcher.Host.Configs;
using VaultSharp.Extensions.Configuration;

namespace CryptoWatcher.Host.Extensions;

public static class VaultExtensions
{
    public static IConfigurationBuilder AddVault(this ConfigurationManager configurationManager)
    {
        var vaultConfig = configurationManager.GetSection(nameof(HashiCorpVaultConfig)).Get<HashiCorpVaultConfig>() ??
                          throw new InvalidOperationException("Vault configuration is not set");

        foreach (var path in vaultConfig.Path)
        {
            configurationManager.AddVaultConfiguration(() =>
                    new VaultOptions(vaultConfig.Address.ToString(), vaultConfig.Token, path, omitVaultKeyName: true),
                vaultConfig.Prefix,
                vaultConfig.Mount);
        }

        return configurationManager;
    }
}