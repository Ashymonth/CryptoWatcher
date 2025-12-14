using CryptoWatcher.Modules.Hyperliquid.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoWatcher.Infrastructure.Configuration.Hyperliquid;

public class HyperliquidVaultEventConfiguration : IEntityTypeConfiguration<HyperliquidPositionCashFlow>
{
    public void Configure(EntityTypeBuilder<HyperliquidPositionCashFlow> builder)
    {
        builder.HasKey(@event => new { @event.VaultAddress, @event.WalletAddress, @event.Date });
    }
}