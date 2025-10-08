using CryptoWatcher.Abstractions;
using CryptoWatcher.Modules.Uniswap.Abstractions;
using CryptoWatcher.Modules.Uniswap.Application.Abstractions;
using CryptoWatcher.Modules.Uniswap.Entities;

namespace CryptoWatcher.Modules.Uniswap.Infrastructure.Services;

public class UniswapChainSynchronizer : IUniswapChainSynchronizer
{
    private readonly IWeb3Factory _web3Factory;
    private readonly IChainLogChunkingStrategy _chunkingStrategy;
    private readonly ICashFlowEventMatcher _eventMatcher;
    private readonly IRepository<PoolPositionCashFlow> _poolPositionCashFlowRepository;

    public UniswapChainSynchronizer(IWeb3Factory web3Factory, IChainLogChunkingStrategy chunkingStrategy,
        ICashFlowEventMatcher eventMatcher, IRepository<PoolPositionCashFlow> poolPositionCashFlowRepository)
    {
        _web3Factory = web3Factory;
        _chunkingStrategy = chunkingStrategy;
        _eventMatcher = eventMatcher;
        _poolPositionCashFlowRepository = poolPositionCashFlowRepository;
    }

    public async Task SynchronizeChainAsync(UniswapChainConfiguration chain, CancellationToken ct = default)
    {
        var web3 = _web3Factory.GetWeb3(chain);

        var lastProcessedBlock = chain.LastProcessedBlock;
        var lastBlockInBlockChain = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();

        foreach (var (from, to) in _chunkingStrategy.CreateChunks(lastProcessedBlock, lastBlockInBlockChain))
        {
            await foreach (var cashFlow in _eventMatcher.FetchCashFlowEvents(chain, from, to, ct))
            {
                await _poolPositionCashFlowRepository.BulkMergeAsync(cashFlow, ct);
            }
        }

        chain.UpdateLastSynchronizedBlock(lastBlockInBlockChain);
    }
}