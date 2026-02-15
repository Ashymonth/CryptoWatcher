using CryptoWatcher.Modules.Hyperliquid.Entities;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization.Abstractions;

public interface IHyperliquidVaultPositionRepository
{
    Task AddOrUpdateAsync(HyperliquidVaultPosition position, CancellationToken ct = default);
    
    Task<HyperliquidVaultPosition?> GetByWalletAsync(EvmAddress wallet, CancellationToken ct = default);
}