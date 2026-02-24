using CryptoWatcher.Extensions;
using CryptoWatcher.Modules.Aave.Application.Abstractions;
using CryptoWatcher.Modules.Aave.Entities;
using CryptoWatcher.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CryptoWatcher.Modules.Aave.Infrastructure.Persistence.Queries;

public class AaveReportQuery : IAaveReportQuery
{
    private readonly AaveDbContext _aaveDbContext;

    public AaveReportQuery(AaveDbContext aaveDbContext)
    {
        _aaveDbContext = aaveDbContext;
    }

    public async Task<IReadOnlyList<AavePosition>> GetPositionsForReportAsync(IReadOnlyCollection<EvmAddress> wallets,
        DateOnly from, DateOnly to, CancellationToken ct)
    {
        return await _aaveDbContext.AavePositions
            .Include(position => position.CashFlows.Where(@event =>
                    @event.Date >= from.ToMinDateTime() && @event.Date <= to.ToMaxDateTime())
                .OrderBy(@event => @event.Date)
            )
            .Include(position =>
                position.Snapshots.Where(snapshot => snapshot.Day >= from && snapshot.Day <= to))
            .Where(position => wallets.Contains(position.WalletAddress) && position.IsActive())
            .ToArrayAsync(ct);
    }
}