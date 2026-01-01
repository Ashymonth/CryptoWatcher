using CryptoWatcher.Abstractions;
using CryptoWatcher.Application.Abstractions;
using CryptoWatcher.Modules.Aave.Entities;
using CryptoWatcher.Modules.Aave.Specifications;
using CryptoWatcher.Shared.Entities;
using Microsoft.Extensions.Logging;

namespace CryptoWatcher.Modules.Aave.Application.Services;

public class AaveDailyPositionPerformanceSynchronizer :
    BaseDailyPositionPerformanceSynchronizer<AavePositionDailyPerformance>,
    IDailyPositionPerformanceSynchronizer
{
    private readonly IRepository<AavePosition> _positionRepository;

    public AaveDailyPositionPerformanceSynchronizer(IRepository<AavePositionDailyPerformance> balanceChangeRepository,
        ILogger<AaveDailyPositionPerformanceSynchronizer> logger,
        IRepository<AavePosition> positionRepository) : base(balanceChangeRepository, logger)
    {
        _positionRepository = positionRepository;
    }

    public string Name => "Aave";

    protected override async Task<List<AavePositionDailyPerformance>> GetDailyBalanceChangesAsync(
        IReadOnlyCollection<Wallet> wallets, DateOnly from, DateOnly to, CancellationToken ct)
    {
        var result = new List<AavePositionDailyPerformance>();

        var positions =
            await _positionRepository.ListAsync(new AavePositionsWithSnapshotsAndEventsSpecification(wallets, from, to),
                ct);

        foreach (var aavePosition in positions)
        {
            AavePositionSnapshot? previousSnapshot = null;
            foreach (var currentSnapshot in aavePosition.Snapshots)
            {
                previousSnapshot ??= currentSnapshot;

                var balanceChange = AavePositionDailyPerformance.Create(aavePosition, previousSnapshot, currentSnapshot);

                result.Add(balanceChange);
            }
        }

        return result;
    }
}