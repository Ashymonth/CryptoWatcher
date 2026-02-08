using CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization.Abstractions;
using CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization.Modes.PositionUpdates;
using CryptoWatcher.Modules.Hyperliquid.Entities;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization;

public class UnprocessedVaultUpdatesFilter : IUnprocessedVaultUpdatesFilter
{
    private readonly IHyperliquidGateway _gateway;

    public UnprocessedVaultUpdatesFilter(IHyperliquidGateway gateway)
    {
        _gateway = gateway;
    }

    public async Task<HyperliquidVaultUpdate[]> GetNewVaultUpdatesAsync(
        EvmAddress walletAddress,
        HyperliquidSynchronizationState state,
        CancellationToken ct = default)
    {
        var updates = await _gateway.GetVaultUpdatesAsync(walletAddress,
            state.LastProcessedEventTimestamp ?? DateTimeOffset.UnixEpoch, ct);

        if (updates.Length == 0 || updates[0].Hash.Equals(state.LastTransactionHash))
        {
            return [];
        }

        return updates.Where(update => !update.Hash.Equals(state.LastTransactionHash)).ToArray();
    }
}