using CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization.Modes;
using CryptoWatcher.Modules.Hyperliquid.Entities;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization.Abstractions;

public interface IHyperliquidVaultPositionUpdater
{
    Task<HyperliquidPositionSyncResult?> UpdatePositionAsync(
        HyperliquidVaultPosition? position,
        EvmAddress walletAddress,
        HyperliquidSynchronizationState state,
        CancellationToken ct = default);
}