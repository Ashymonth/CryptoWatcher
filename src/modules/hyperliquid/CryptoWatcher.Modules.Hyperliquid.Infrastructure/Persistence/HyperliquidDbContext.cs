using CryptoWatcher.Modules.Hyperliquid.Entities;
using CryptoWatcher.Modules.Infrastructure.Shared.Persistence;
using Microsoft.EntityFrameworkCore;
using SmartEnum.EFCore;

namespace CryptoWatcher.Modules.Hyperliquid.Infrastructure.Persistence;

public class HyperliquidDbContext : BaseDbContext
{
    public HyperliquidDbContext(DbContextOptions<HyperliquidDbContext> options) : base(options)
    {
    }

    public DbSet<HyperliquidVaultPosition> HyperliquidVaultPositions => Set<HyperliquidVaultPosition>();

    public DbSet<HyperliquidVaultPositionSnapshot> HyperliquidVaultPositionSnapshots =>
        Set<HyperliquidVaultPositionSnapshot>();

    public DbSet<HyperliquidPositionDailyPerformance> HyperliquidPositionDailyPerformances =>
        Set<HyperliquidPositionDailyPerformance>();

    public DbSet<HyperliquidPositionCashFlow> HyperliquidPositionCashFlows => Set<HyperliquidPositionCashFlow>();

    public DbSet<HyperliquidSynchronizationState> HyperliquidSynchronizationStates =>
        Set<HyperliquidSynchronizationState>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("hyperliquid");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HyperliquidDbContext).Assembly);
        modelBuilder.ConfigureSmartEnum();
    }
}