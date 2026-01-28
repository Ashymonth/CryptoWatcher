using CryptoWatcher.Abstractions;
using CryptoWatcher.Modules.Uniswap.Application.Abstractions;
using CryptoWatcher.Modules.Uniswap.Entities;
using CryptoWatcher.Shared.Entities;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Uniswap.Application.Services.Synchronization.TransactionSynchronization;

public class UniswapPositionTransactionSynchronizer : IUniswapPositionTransactionSynchronizer
{
    private readonly IUniswapPositionFromTransactionUpdater _updater;
    private readonly IRepository<UniswapLiquidityPosition> _positionsRepository;

    public UniswapPositionTransactionSynchronizer(IUniswapPositionFromTransactionUpdater updater,
        IRepository<UniswapLiquidityPosition> positionsRepository)
    {
        _updater = updater;
        _positionsRepository = positionsRepository;
    }

    public async Task SynchronizeEventFromTransactionAsync(UniswapChainConfiguration chain,
        Wallet wallet,
        TransactionHash transactionHash,
        CancellationToken ct = default)
    {
        var positions = await _updater.ApplyEventFromTransactionAsync(chain, wallet, transactionHash, ct);

        await _positionsRepository.BulkMergeAsync(positions, ct);
    }
}