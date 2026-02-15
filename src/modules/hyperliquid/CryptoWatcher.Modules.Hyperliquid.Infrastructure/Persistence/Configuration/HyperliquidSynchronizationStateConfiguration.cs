using CryptoWatcher.Modules.Hyperliquid.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoWatcher.Modules.Hyperliquid.Infrastructure.Persistence.Configuration;

public class HyperliquidSynchronizationStateConfiguration : IEntityTypeConfiguration<HyperliquidSynchronizationState>
{
    public void Configure(EntityTypeBuilder<HyperliquidSynchronizationState> builder)
    {
        builder.HasKey(state => state.WalletAddress);
    }
}