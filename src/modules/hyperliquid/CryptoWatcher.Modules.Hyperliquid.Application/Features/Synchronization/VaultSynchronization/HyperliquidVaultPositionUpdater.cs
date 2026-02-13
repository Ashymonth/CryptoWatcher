using CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization.Abstractions;
using CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization.Models;
using CryptoWatcher.Modules.Hyperliquid.Entities;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization;

public class HyperliquidVaultPositionUpdater : IHyperliquidVaultPositionUpdater
{
    private readonly IUnprocessedVaultUpdatesFilter _unprocessedVaultUpdatesFilter;

    public HyperliquidVaultPositionUpdater(IUnprocessedVaultUpdatesFilter unprocessedVaultUpdatesFilter)
    {
        _unprocessedVaultUpdatesFilter = unprocessedVaultUpdatesFilter;
    }

    public async Task<HyperliquidPositionSyncResult?> UpdatePositionAsync(
        HyperliquidVaultPosition? position,
        EvmAddress walletAddress,
        HyperliquidSynchronizationState state,
        CancellationToken ct = default)
    {
        var vaultUpdates = await _unprocessedVaultUpdatesFilter.GetNewVaultUpdatesAsync(walletAddress, state, ct);

        if (vaultUpdates.Length == 0 && position is null)
        {
            return null;
        } 
        
        if (position is null)
        {
            if (vaultUpdates.Length == 0)
            {
                throw new InvalidOperationException(
                    $"Vault exists for wallet {walletAddress} but no transaction history found in range [{state.LastProcessedEventTimestamp:yyyy-MM-dd}");
            }

            position = HyperliquidVaultPosition.Open(walletAddress, HyperliquidWellKnowFields.HlpVaultAddress);
        }

        foreach (var update in vaultUpdates)
        {
            position.AddCashFlowIfNotExists(update.Amount, update.GetCashFlowEvent(), update.Timestamp, update.Hash);
        }

        return new HyperliquidPositionSyncResult
        {
            Position = position,
            LastHyperliquidVaultUpdate = vaultUpdates.Length != 0 ? vaultUpdates[^1] : null
        };
    }
}