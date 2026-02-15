using CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization.Models.PositionUpdates;
using CryptoWatcher.Modules.Hyperliquid.Entities;

namespace CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization.Models;

public class HyperliquidPositionSyncResult
{
    public HyperliquidVaultPosition Position { get; init; } = null!;

    public HyperliquidVaultUpdate? LastHyperliquidVaultUpdate { get; init; }
}