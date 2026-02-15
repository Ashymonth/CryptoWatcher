using CryptoWatcher.Modules.Hyperliquid.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoWatcher.Modules.Hyperliquid.Infrastructure.Persistence.Configuration;

public class
    HyperliquidPositionDailyPerformanceConfiguration : IEntityTypeConfiguration<HyperliquidPositionDailyPerformance>
{
    public void Configure(EntityTypeBuilder<HyperliquidPositionDailyPerformance> builder)
    {
        builder.HasKey(performance => new { performance.VaultAddress, performance.WalletAddress, performance.Day });
    }
}