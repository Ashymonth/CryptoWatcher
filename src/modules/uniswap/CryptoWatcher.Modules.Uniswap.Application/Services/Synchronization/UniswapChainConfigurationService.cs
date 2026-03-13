using CryptoWatcher.Abstractions;
using CryptoWatcher.Modules.Uniswap.Entities;
using CryptoWatcher.Modules.Uniswap.Specifications;
using Microsoft.Extensions.Caching.Memory;

namespace CryptoWatcher.Modules.Uniswap.Application.Services.Synchronization;

public class UniswapChainConfigurationService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IRepository<UniswapChainConfiguration> _chainConfigurationRepository;

    public UniswapChainConfigurationService(IMemoryCache memoryCache,
        IRepository<UniswapChainConfiguration> chainConfigurationRepository)
    {
        _memoryCache = memoryCache;
        _chainConfigurationRepository = chainConfigurationRepository;
    }

    public async ValueTask<UniswapChainConfiguration> GetByIdAsync(int chainId, CancellationToken ct)
    {
        return (await _memoryCache.GetOrCreateAsync(chainId,
            async _ => await _chainConfigurationRepository.FirstOrDefaultAsync(new GetUnichainSpecification(chainId),
                ct)))!;
    }
}