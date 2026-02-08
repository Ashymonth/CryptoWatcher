using CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization.Modes.PositionUpdates;
using CryptoWatcher.Modules.Hyperliquid.Entities;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization.Abstractions;

public interface IUnprocessedVaultUpdatesFilter
{
    Task<HyperliquidVaultUpdate[]> GetNewVaultUpdatesAsync(EvmAddress walletAddress,
        HyperliquidSynchronizationState state,
        CancellationToken ct = default);
}