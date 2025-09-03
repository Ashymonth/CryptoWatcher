using Ardalis.Specification;
using CryptoWatcher.UniswapModule.Entities;

namespace CryptoWatcher.UniswapModule.Specifications;

internal sealed class UniswapPositionsForReportSpecification : Specification<PoolPosition>
{
    public UniswapPositionsForReportSpecification(DateOnly from, DateOnly to)
    {
        Query
            .Include(poolPosition => poolPosition.PoolPositionSnapshots
                .Where(snapshot => snapshot.Day >= from && snapshot.Day <= to)
                .OrderBy(snapshot => snapshot.Day)
            );
    }
}