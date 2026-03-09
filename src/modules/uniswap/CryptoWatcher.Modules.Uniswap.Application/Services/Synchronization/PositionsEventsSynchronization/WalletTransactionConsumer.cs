using System.Runtime.CompilerServices;
using CryptoWatcher.Abstractions;
using CryptoWatcher.Modules.Uniswap.Application.Abstractions;
using CryptoWatcher.Modules.Uniswap.Application.Models;
using CryptoWatcher.Modules.Uniswap.Entities;

namespace CryptoWatcher.Modules.Uniswap.Application.Services.Synchronization.PositionsEventsSynchronization;

public class WalletTransactionConsumer : IWalletTransactionConsumer
{
    private readonly UniswapChainConfigurationService _chainConfigurationService;
    private readonly IUniswapWalletEventApplier _walletEventApplier;
    private readonly IRepository<UniswapLiquidityPosition> _repository;

    public WalletTransactionConsumer(UniswapChainConfigurationService chainConfigurationService,
        IUniswapWalletEventApplier walletEventApplier, IRepository<UniswapLiquidityPosition> repository)
    {
        _chainConfigurationService = chainConfigurationService;
        _walletEventApplier = walletEventApplier;
        _repository = repository;
    }

    public async IAsyncEnumerable<BlockchainTransaction[]> ConsumeTransactionsAsync(
        IEnumerable<BlockchainTransaction> blockchainTransaction,
        [EnumeratorCancellation] CancellationToken ct = default)
    {
        foreach (var transactionByChain in blockchainTransaction.GroupBy(transaction => transaction.ChainId))
        {
            var chain = await _chainConfigurationService.GetByIdAsync(transactionByChain.Key, ct);

            foreach (var transactionsByFrom in transactionByChain.GroupBy(transaction => transaction.From))
            {
                await foreach (var updatedPositions in _walletEventApplier.ApplyEventsToPositionsAsync(chain,
                                   transactionsByFrom, ct))
                {
                    await _repository.BulkMergeAsync(updatedPositions, ct);
                    yield return transactionsByFrom.ToArray();
                }
            }
        }
    }
}