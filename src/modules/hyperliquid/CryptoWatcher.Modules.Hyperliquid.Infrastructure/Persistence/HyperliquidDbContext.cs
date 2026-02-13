using CryptoWatcher.Modules.Hyperliquid.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoWatcher.Modules.Hyperliquid.Infrastructure.Persistence;

public class HyperliquidDbContext : DbContext
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
    }
}