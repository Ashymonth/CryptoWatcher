using System.Numerics;
using System.Runtime.CompilerServices;
using CryptoWatcher.Modules.Uniswap.Abstractions;
using CryptoWatcher.Modules.Uniswap.Application.Abstractions;
using CryptoWatcher.Modules.Uniswap.Entities;
using CryptoWatcher.Modules.Uniswap.Models;
using Microsoft.Extensions.Logging;

namespace CryptoWatcher.Modules.Uniswap.Infrastructure.Services;

internal class LiquidityEventsProvider : ILiquidityEventsProvider
{
    private readonly IBlockchainLogProvider _logProvider;
    private readonly ITransactionDataProvider _transactionDataProvider;
    private readonly ILiquidityPoolEventDecoder _eventDecoder;
    private readonly ILogger<LiquidityEventsProvider> _logger;

    public LiquidityEventsProvider(IBlockchainLogProvider logProvider, ITransactionDataProvider transactionDataProvider,
        ILiquidityPoolEventDecoder eventDecoder, ILogger<LiquidityEventsProvider> logger)
    {
        _logProvider = logProvider;
        _transactionDataProvider = transactionDataProvider;
        _eventDecoder = eventDecoder;
        _logger = logger;
    }

    public async IAsyncEnumerable<List<LiquidityPoolPositionEvent>> FetchLiquidityPoolEvents(
        UniswapChainConfiguration chain,
        BigInteger fromBlock,
        BigInteger toBlock,
        [EnumeratorCancellation] CancellationToken ct = default)
    {
        var blockchainLogBatch = await _logProvider.GetLogsAsync(chain, fromBlock, toBlock);
        var result = new List<LiquidityPoolPositionEvent?>();
        var tasks = blockchainLogBatch.Logs.Select(async log =>
        {
            try
            {
                var transactionData =
                    await _transactionDataProvider.GetTransactionDataAsync(chain, log.TransactionHash, ct);

                return _eventDecoder.DecodeModifyLiquidityEvent(transactionData.WalletAddress, log.Data,
                    transactionData.TokenPair);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process log {TransactionHash}", log.TransactionHash);
                return null;
            }
        });

        var events = await Task.WhenAll(tasks);
        result.AddRange(events.Where(e => e != null));

        yield return result!;
    }
}