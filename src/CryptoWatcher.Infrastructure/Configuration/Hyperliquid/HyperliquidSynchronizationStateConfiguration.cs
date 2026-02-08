using CryptoWatcher.Modules.Hyperliquid.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoWatcher.Infrastructure.Configuration.Hyperliquid;

public class HyperliquidSynchronizationStateConfiguration : IEntityTypeConfiguration<HyperliquidSynchronizationState>
{
    public void Configure(EntityTypeBuilder<HyperliquidSynchronizationState> builder)
    {
        builder.HasKey(state => state.WalletAddress);
    }
}