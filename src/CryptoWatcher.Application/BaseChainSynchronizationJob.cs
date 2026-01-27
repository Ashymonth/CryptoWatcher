using CryptoWatcher.Abstractions;
using CryptoWatcher.Shared.Entities;

namespace CryptoWatcher.Application;

public abstract class BaseChainSynchronizationJob<TChain, TContext> where TChain : BaseChainConfiguration
{
    private readonly IRepository<Wallet> _walletRepository;
    private readonly IRepository<TChain> _chainRepository;

    protected BaseChainSynchronizationJob(IRepository<Wallet> walletRepository, IRepository<TChain> chainRepository)
    {
        _walletRepository = walletRepository;
        _chainRepository = chainRepository;
    }

    public async Task SynchronizeAsync(CancellationToken ct = default)
    {
        var wallets = await _walletRepository.ListAsync(ct);

        var chains = await _chainRepository.ListAsync(ct);

        var context = await CreateContextAsync(ct);

        foreach (var wallet in wallets)
        {
            foreach (var chain in chains)
            {
                await SynchronizeWalletOnChainAsync(chain, wallet, context, ct);
            }
        }
    }

    protected abstract Task<TContext> CreateContextAsync(CancellationToken ct);

    protected abstract Task SynchronizeWalletOnChainAsync(TChain chain, Wallet wallet, TContext context,
        CancellationToken ct);
}