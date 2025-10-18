using CryptoWatcher.Infrastructure.Extensions;
using CryptoWatcher.Modules.Aave.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoWatcher.Infrastructure.Configuration.Aave;

public class AaveChainConfigurationConfiguration : IEntityTypeConfiguration<AaveChainConfiguration>
{
    public void Configure(EntityTypeBuilder<AaveChainConfiguration> builder)
    {
        builder.HasKey(configuration => configuration.Name);

        builder.Property(configuration => configuration.Name).HasMaxLength(32);
        builder.Property(configuration => configuration.RpcAuthToken).HasMaxLength(64);
        builder.Property(configuration => configuration.RpcUrl)
            .HasConversion(uri => uri.ToString(), uriString => new Uri(uriString))
            .HasMaxLength(128);

        builder.ComplexProperty(configuration => configuration.SmartContractAddresses, propertyBuilder =>
        {
            propertyBuilder.Property(addresses => addresses.PoolAddressesProviderAddress).ConfigureEvmAddress();
            propertyBuilder.Property(addresses => addresses.UiPoolDataProviderAddress).ConfigureEvmAddress();
        });
    }
}