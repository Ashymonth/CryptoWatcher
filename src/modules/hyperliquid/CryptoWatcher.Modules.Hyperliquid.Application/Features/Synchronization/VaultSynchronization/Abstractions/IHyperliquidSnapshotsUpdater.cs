using CryptoWatcher.Modules.Hyperliquid.Entities;

namespace CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization.Abstractions;

public interface IHyperliquidSnapshotsUpdater
{
    Task TakeVaultBalanceSnapshotAsync(HyperliquidVaultPosition position, DateTime syncDate,
        CancellationToken ct = default);
}