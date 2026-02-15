using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization.Abstractions;

public interface IHyperliquidVaultPositionSyncJob
{
    Task SyncPositionAsync(EvmAddress walletAddress, DateTime snapshotDate, CancellationToken ct = default);
}