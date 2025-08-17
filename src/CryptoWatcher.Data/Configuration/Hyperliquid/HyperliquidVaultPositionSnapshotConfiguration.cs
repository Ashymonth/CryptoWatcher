using CryptoWatcher.Entities.Hyperliquid;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoWatcher.Data.Configuration.Hyperliquid;

public class HyperliquidVaultPositionSnapshotConfiguration : IEntityTypeConfiguration<HyperliquidVaultPositionSnapshot>
{
    public void Configure(EntityTypeBuilder<HyperliquidVaultPositionSnapshot> builder)
    {
        builder.HasKey(snapshot => new { snapshot.VaultAddress, Date = snapshot.Day });
        
        builder.Property(wallet => wallet.VaultAddress).HasMaxLength(64);
    }
}