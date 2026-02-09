using CryptoWatcher.Abstractions;
using CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization.Abstractions;
using CryptoWatcher.Modules.Hyperliquid.Entities;
using CryptoWatcher.Modules.Hyperliquid.Specifications;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Hyperliquid.Infrastructure.Repositories;

public class HyperliquidVaultPositionRepository : IHyperliquidVaultPositionRepository
{
    private readonly IRepository<HyperliquidVaultPosition> _repository;   

    public HyperliquidVaultPositionRepository(IRepository<HyperliquidVaultPosition> repository)
    {
        _repository = repository;
    }

    public async Task AddOrUpdateAsync(HyperliquidVaultPosition position, CancellationToken ct = default)
    {
        await _repository.BulkMergeAsync([position], ct);
    }

    public async Task<HyperliquidVaultPosition?> GetByWalletAsync(EvmAddress wallet, CancellationToken ct = default)
    {
        return await _repository.FirstOrDefaultAsync(new HyperliquidPositionsWithSnapshotsAndCashFlowByWallet(wallet),
            ct);
    }
}