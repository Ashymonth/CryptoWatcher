using CryptoWatcher.Modules.Hyperliquid.Entities;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization.Abstractions;

public interface IHyperliquidSyncStateRepository
{
    Task AddOrUpdateAsync(HyperliquidSynchronizationState state, CancellationToken ct = default);
    
    Task<HyperliquidSynchronizationState?> GetByWalletAsync(EvmAddress wallet, CancellationToken ct = default);
}