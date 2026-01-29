using System.Runtime.CompilerServices;
using CryptoWatcher.Modules.Uniswap.Application.Abstractions;
using CryptoWatcher.Modules.Uniswap.Application.Models;
using CryptoWatcher.Modules.Uniswap.Entities;
using CryptoWatcher.Shared.Entities;

namespace CryptoWatcher.Modules.Uniswap.Application.Services.Synchronization.PositionsEventsSynchronization;

public class UniswapWalletEventApplier : IUniswapWalletEventApplier
{
    private const int ChunkSize = 50;

    private readonly IUniswapWalletTransactionScanner _uniswapWalletTransactionScanner;
    private readonly IUniswapPositionUpdater _positionUpdater;
    
    public UniswapWalletEventApplier(IUniswapWalletTransactionScanner uniswapWalletTransactionScanner,
        IUniswapPositionUpdater positionUpdater)
    {
        _uniswapWalletTransactionScanner = uniswapWalletTransactionScanner;
        _positionUpdater = positionUpdater;
    }

    public async IAsyncEnumerable<WalletEventExtractionResult> ApplyEventsToPositionsAsync(
        UniswapChainConfiguration chainConfiguration,
        UniswapSynchronizationState synchronizationState,
        Wallet wallet,
        [EnumeratorCancellation] CancellationToken ct = default)
    {
        var scannedTransactions = _uniswapWalletTransactionScanner
            .ScanWalletTransactionsAsync(chainConfiguration, synchronizationState, wallet.Address, ct)
            .ToBlockingEnumerable(cancellationToken: ct); // until a problem with pending transaction is fixed

        foreach (var uniswapEventBatch in scannedTransactions.Chunk(ChunkSize))
        {
            var uniswapEvents = uniswapEventBatch
                .Where(item => item.Event is not null)
                .Select(eventScanItem => eventScanItem.Event!)
                .ToArray();

            var updatedPositions = Array.Empty<UniswapLiquidityPosition>();

            if (uniswapEvents.Length > 0)
            {
                updatedPositions = await _positionUpdater.UpdateFromEventAsync(chainConfiguration, wallet.Address,
                    uniswapEvents, ct);
            }

            yield return new WalletEventExtractionResult
            {
                UpdatedPositions = updatedPositions,
                LastScannedTransaction =
                    uniswapEventBatch.MaxBy(x => x.Transaction.BlockNumber)!.Transaction
            };
        }
    }
}