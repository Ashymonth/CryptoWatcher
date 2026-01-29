using CryptoWatcher.Modules.Uniswap.Application.Abstractions;
using TickerQ.Utilities.Base;

namespace CryptoWatcher.Infrastructure.CronJobs.Uniswap;

public class SyncUniswapEventsCronJob
{
    private readonly IUniswapWalletPositionsSyncJob _chainSynchronizerJob;

    public SyncUniswapEventsCronJob(IUniswapWalletPositionsSyncJob chainSynchronizerJob)
    {
        _chainSynchronizerJob = chainSynchronizerJob;
    }

    [TickerFunction(nameof(SyncUniswapEventsAsync), "* * * * *")]
    public async Task SyncUniswapEventsAsync()
    {
        await _chainSynchronizerJob.SynchronizeAsync();
    }
}