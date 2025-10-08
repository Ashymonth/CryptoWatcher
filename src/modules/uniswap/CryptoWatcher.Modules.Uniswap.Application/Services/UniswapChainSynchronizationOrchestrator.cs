using CryptoWatcher.Abstractions;
using CryptoWatcher.Modules.Uniswap.Application.Abstractions;
using CryptoWatcher.Modules.Uniswap.Entities;
using CryptoWatcher.Modules.Uniswap.Specifications;

namespace CryptoWatcher.Modules.Uniswap.Application.Services;

public class UniswapChainSynchronizationOrchestrator : IUniswapChainSynchronizerOrchestrator
{
    private readonly IUniswapChainSynchronizer _chainSynchronizer;
    private readonly IRepository<UniswapChainConfiguration> _chainConfigurationRepository;
    
    public UniswapChainSynchronizationOrchestrator(IUniswapChainSynchronizer chainSynchronizer, IRepository<UniswapChainConfiguration> chainConfigurationRepository)
    {
        _chainSynchronizer = chainSynchronizer;
        _chainConfigurationRepository = chainConfigurationRepository;
    }

    public async Task SynchronizeAllChainsAsync(CancellationToken ct = default)
    {
        var chainsToSynchronize =
            await _chainConfigurationRepository.ListAsync(new GetUniswapChainWithStateAndActivePositions(), ct);
        
        foreach (var uniswapChainConfiguration in chainsToSynchronize)
        {
            await _chainConfigurationRepository.UnitOfWork.BeginTransactionAsync(ct);
            
            try
            {
                await _chainSynchronizer.SynchronizeChainAsync(uniswapChainConfiguration, ct);
                
                await _chainConfigurationRepository.UnitOfWork.CommitTransactionAsync(ct);
            }
            catch (Exception)
            {
                await _chainConfigurationRepository.UnitOfWork.RollbackTransactionAsync(ct);
                throw;
            }
        }
    }
}