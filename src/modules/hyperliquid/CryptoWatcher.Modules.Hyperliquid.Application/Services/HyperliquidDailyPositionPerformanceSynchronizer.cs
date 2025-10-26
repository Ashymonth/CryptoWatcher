using CryptoWatcher.Abstractions;
using CryptoWatcher.Application.Abstractions;
using CryptoWatcher.Modules.Hyperliquid.Entities;
using CryptoWatcher.Modules.Hyperliquid.Specifications;
using CryptoWatcher.Shared.Entities;
using Microsoft.Extensions.Logging;

namespace CryptoWatcher.Modules.Hyperliquid.Application.Services;

public class HyperliquidDailyPositionPerformanceSynchronizer :
    BaseDailyPositionPerformanceSynchronizer<HyperliquidPositionDailyPerformance>,
    IDailyPositionPerformanceSynchronizer
{
    private readonly IRepository<HyperliquidVaultPosition> _positionRepository;

    public HyperliquidDailyPositionPerformanceSynchronizer(IRepository<HyperliquidPositionDailyPerformance> balanceChangeRepository,
        IRepository<HyperliquidVaultPosition> positionRepository,
        ILogger<HyperliquidDailyPositionPerformanceSynchronizer> logger) : base(balanceChangeRepository, logger)
    {
        _positionRepository = positionRepository;
    }

    public string Name => "Hyperliquid";

    protected override async Task<List<HyperliquidPositionDailyPerformance>> GetDailyBalanceChangesAsync(
        IReadOnlyCollection<Wallet> wallets, DateOnly from, DateOnly to, CancellationToken ct)
    {
        var positions =
            await _positionRepository.ListAsync(new HyperliquidPositionsForReportSpecification(wallets, from, to), ct);

        var result = new List<HyperliquidPositionDailyPerformance>();
        foreach (var vaultPosition in positions)
        {
            HyperliquidVaultPositionSnapshot? previousSnapshot = null;
            foreach (var currentSnapshot in vaultPosition.PositionSnapshots)
            {
                previousSnapshot ??= currentSnapshot;

                var balanceChange =
                    HyperliquidPositionDailyPerformance.CreateFromSnapshot(vaultPosition, previousSnapshot, currentSnapshot);

                previousSnapshot = currentSnapshot;

                result.Add(balanceChange);
            }
        }

        return result;
    }
}