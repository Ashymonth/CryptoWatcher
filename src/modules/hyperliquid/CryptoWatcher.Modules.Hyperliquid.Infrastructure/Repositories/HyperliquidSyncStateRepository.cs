using CryptoWatcher.Abstractions;
using CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization.Abstractions;
using CryptoWatcher.Modules.Hyperliquid.Entities;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Hyperliquid.Infrastructure.Repositories;

public class HyperliquidSyncStateRepository : IHyperliquidSyncStateRepository
{
    private readonly IRepository<HyperliquidSynchronizationState> _repository;

    public HyperliquidSyncStateRepository(IRepository<HyperliquidSynchronizationState> repository)
    {
        _repository = repository;
    }

    public async Task AddOrUpdateAsync(HyperliquidSynchronizationState state, CancellationToken ct = default)
    {
        await _repository.BulkMergeAsync([state], ct);
    }

    public async Task<HyperliquidSynchronizationState?> GetByWalletAsync(EvmAddress wallet, CancellationToken ct = default)
    {
        return await _repository.GetByIdAsync(wallet, ct);
    }
}