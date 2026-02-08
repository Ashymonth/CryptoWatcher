using CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization.Modes;
using CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization.Modes.PositionUpdates;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization.Abstractions;

public interface IHyperliquidGateway
{
    Task<HyperliquidVaultUpdate[]> GetVaultUpdatesAsync(EvmAddress walletAddress,
        DateTimeOffset from,
        CancellationToken ct = default);

    Task<IReadOnlyCollection<HyperliquidVault>> GetVaultsPositionsEquityAsync(EvmAddress walletAddress,
        CancellationToken ct = default);
}