using CryptoWatcher.Modules.Hyperliquid.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoWatcher.Infrastructure.Configuration.Hyperliquid;

public class HyperliquidVaultPositionSnapshotConfiguration : IEntityTypeConfiguration<HyperliquidVaultPositionSnapshot>
{
    public void Configure(EntityTypeBuilder<HyperliquidVaultPositionSnapshot> builder)
    {
        builder.HasKey(snapshot => new { snapshot.VaultAddress, snapshot.WalletAddress, Date = snapshot.Day });
    }
}