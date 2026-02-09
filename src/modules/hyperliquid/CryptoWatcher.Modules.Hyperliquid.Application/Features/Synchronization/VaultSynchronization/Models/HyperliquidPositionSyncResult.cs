using CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization.Modes.PositionUpdates;
using CryptoWatcher.Modules.Hyperliquid.Entities;

namespace CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization.Modes;

public class HyperliquidPositionSyncResult
{
    public HyperliquidVaultPosition Position { get; init; } = null!;

    public HyperliquidVaultUpdate? LastHyperliquidVaultUpdate { get; init; }
}