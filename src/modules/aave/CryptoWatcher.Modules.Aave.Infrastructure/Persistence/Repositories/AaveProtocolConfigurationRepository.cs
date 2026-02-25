using CryptoWatcher.Modules.Aave.Application.Abstractions;
using CryptoWatcher.Modules.Aave.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoWatcher.Modules.Aave.Infrastructure.Persistence.Repositories;

public class AaveProtocolConfigurationRepository : IAaveProtocolConfigurationRepository
{
    private readonly AaveDbContext _dbContext;

    public AaveProtocolConfigurationRepository(AaveDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<AaveProtocolConfiguration>> GetAaveProtocolConfigurationsAsync(
        CancellationToken ct = default)
    {
        return await _dbContext.AaveProtocolConfigurations.ToArrayAsync(ct);
    }
}