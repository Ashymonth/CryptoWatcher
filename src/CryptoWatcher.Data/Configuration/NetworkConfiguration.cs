using CryptoWatcher.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoWatcher.Data.Configuration;

public class NetworkConfiguration : IEntityTypeConfiguration<Network>
{
    public void Configure(EntityTypeBuilder<Network> builder)
    {
        builder.HasKey(network => network.Name);

        builder.Property(network => network.Name).HasMaxLength(32);
        builder.Property(network => network.RpcUrl).HasMaxLength(128);
        builder.Property(network => network.MultiCallAddress).HasMaxLength(256);
        builder.Property(network => network.NftManagerAddress).HasMaxLength(266);
        builder.Property(network => network.PoolFactoryAddress).HasMaxLength(256);

        builder.HasMany(x => x.LiquidityPoolPositions)
            .WithOne(position => position.Network)
            .HasForeignKey(position => position.NetworkName)
            .IsRequired();
    }
}