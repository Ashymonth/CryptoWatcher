using CryptoWatcher.Abstractions;
using CryptoWatcher.Application;
using CryptoWatcher.Modules.Uniswap.Application.Abstractions;
using CryptoWatcher.Modules.Uniswap.Application.Services.Synchronization.PositionsPriceSync.Models;
using CryptoWatcher.Modules.Uniswap.Entities;
using CryptoWatcher.Shared.Entities;

namespace CryptoWatcher.Modules.Uniswap.Application.Services.Synchronization.PositionsPriceSync;

public class PositionPriceSynchronizationJob :
    BaseChainSynchronizationJob<UniswapChainConfiguration, UniswapPositionsContext>, IPositionPriceSynchronizationJob
{
    private const int ChunkSize = 500;

    private readonly IRepository<UniswapLiquidityPosition> _positionsRepository;
    private readonly IPositionPriceSynchronizer _positionPriceSynchronizer;

    public PositionPriceSynchronizationJob(IRepository<Wallet> walletRepository,
        IRepository<UniswapChainConfiguration> chainRepository,
        IRepository<UniswapLiquidityPosition> positionsRepository,
        IPositionPriceSynchronizer positionPriceSynchronizer) : base(walletRepository, chainRepository)
    {
        _positionsRepository = positionsRepository;
        _positionPriceSynchronizer = positionPriceSynchronizer;
    }

    protected override async Task<UniswapPositionsContext> CreateContextAsync(CancellationToken ct)
    {
        var positions = await _positionsRepository.ListAsync(ct);

        return new UniswapPositionsContext(positions);
    }

    protected override async Task SynchronizeWalletOnChainAsync(UniswapChainConfiguration chain, Wallet wallet,
        UniswapPositionsContext context,
        CancellationToken ct)
    {
        var chainPositions = context.ForChainAndWallet(chain, wallet);

        if (chainPositions.Length == 0)
        {
            return;
        }

        var day = DateOnly.FromDateTime(DateTime.UtcNow);

        var updatedPositions = _positionPriceSynchronizer.SynchronizeAsync(
            chain,
            wallet,
            chainPositions,
            day,
            ct);

        await foreach (var chunk in updatedPositions.Chunk(ChunkSize).WithCancellation(ct))
        {
            await _positionsRepository.BulkMergeAsync(chunk, ct);
        }
    }
}