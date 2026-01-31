using CryptoWatcher.Modules.Uniswap.Application.Abstractions;
using Hangfire.RecurringJobExtensions;
using JetBrains.Annotations;

namespace CryptoWatcher.Infrastructure.CronJobs.Uniswap;

[UsedImplicitly]
public class SyncUniswapPositionsCronJob
{
    private readonly IUniswapWalletPositionsSyncJob _chainSynchronizerJob;

    public SyncUniswapPositionsCronJob(IUniswapWalletPositionsSyncJob chainSynchronizerJob)
    {
        _chainSynchronizerJob = chainSynchronizerJob;
    }

    [RecurringJob(CronRegistry.EveryMinute, RecurringJobId = nameof(SyncUniswapPositions))]
    public async Task SyncUniswapPositions()
    {
        await _chainSynchronizerJob.SynchronizeAsync();
    }
}