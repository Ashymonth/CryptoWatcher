using CryptoWatcher.Modules.Hyperliquid.Entities;

namespace CryptoWatcher.Modules.Hyperliquid.Application.Abstractions;

public interface IHyperliquidSyncRepoFacade
{
    Task SavePositionWithGraphAsync(IReadOnlyCollection<HyperliquidVaultPosition> vaults,
        CancellationToken ct = default);
}