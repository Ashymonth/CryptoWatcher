using CryptoWatcher.Modules.Uniswap.Application.Abstractions;
using Hangfire.RecurringJobExtensions;
using JetBrains.Annotations;

namespace CryptoWatcher.Infrastructure.CronJobs.Uniswap;

[UsedImplicitly]
public class SyncUniswapPositionSnapshotsCronJob
{
    private readonly IPositionPriceSynchronizationJob _positionsSyncService;

    public SyncUniswapPositionSnapshotsCronJob(IPositionPriceSynchronizationJob positionsSyncService)
    {
        _positionsSyncService = positionsSyncService;
    }
    
    [RecurringJob(CronRegistry.Every50Minutes, RecurringJobId = nameof(SyncUniswapPositionSnapshots))]
    public async Task SyncUniswapPositionSnapshots(CancellationToken ct)
    {
        await _positionsSyncService.SynchronizeAsync(ct);
    }
}