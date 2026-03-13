using System.Runtime.CompilerServices;
using CryptoWatcher.Modules.Uniswap.Application.Abstractions;
using CryptoWatcher.Modules.Uniswap.Application.Models;
using CryptoWatcher.Modules.Uniswap.Entities;

namespace CryptoWatcher.Modules.Uniswap.Application.Services.Synchronization.PositionsEventsSynchronization;

public class UniswapWalletEventApplier : IUniswapWalletEventApplier
{
    private readonly IUniswapTransactionEnricher _transactionEnricher;
    private readonly IUniswapPositionUpdater _positionUpdater;

    public UniswapWalletEventApplier(IUniswapPositionUpdater positionUpdater,
        IUniswapTransactionEnricher transactionEnricher)
    {
        _positionUpdater = positionUpdater;
        _transactionEnricher = transactionEnricher;
    }

    public async IAsyncEnumerable<UniswapLiquidityPosition[]> ApplyEventsToPositionsAsync(
        UniswapChainConfiguration chainConfiguration,
        IEnumerable<BlockchainTransaction> transaction,
        [EnumeratorCancellation] CancellationToken ct = default)
    {
        var tasks = transaction
            .OrderBy(blockchainTransaction => blockchainTransaction.Timestamp)
            .Select(tx => _transactionEnricher.TryEnrichAsync(chainConfiguration, tx, ct));

        foreach (var chunk in tasks.Chunk(5))
        {
            var results = await Task.WhenAll(chunk);

            var uniswapEvents = results
                .Where(x => x != null)
                .ToArray();

            if (uniswapEvents.Length != 0)
            {
                yield return await _positionUpdater.UpdateFromEventAsync(chainConfiguration, uniswapEvents!, ct);
            }    
        }
        
    }
}