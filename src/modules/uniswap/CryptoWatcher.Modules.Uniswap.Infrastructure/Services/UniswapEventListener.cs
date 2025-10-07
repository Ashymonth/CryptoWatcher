using System.Numerics;
using System.Runtime.CompilerServices;
using CryptoWatcher.Modules.Uniswap.Abstractions;
using CryptoWatcher.Modules.Uniswap.Entities;
using CryptoWatcher.UniswapModule.Models;

namespace CryptoWatcher.Modules.Uniswap.Infrastructure.Services;

internal class UnichainEventFetcher : IUnichainEventFetcher
{
    private readonly IUnichainLogProvider _unichainLogProvider;
    private readonly ILiquidityPoolEventDecoder _liquidityPoolEventDecoder;
    private readonly IUnichainLogReader _unichainLogReader;
    private readonly IWeb3Factory  _web3Factory;
    
    public UnichainEventFetcher(
        IUnichainLogProvider unichainLogProvider,
        ILiquidityPoolEventDecoder liquidityPoolEventDecoder, IUnichainLogReader unichainLogReader, IWeb3Factory web3Factory)
    {
        _unichainLogProvider = unichainLogProvider;
        _liquidityPoolEventDecoder = liquidityPoolEventDecoder;
        _unichainLogReader = unichainLogReader;
        _web3Factory = web3Factory;
    }

    public async IAsyncEnumerable<List<LiquidityPoolPositionEvent>> FetchLiquidityPoolEvents(
        UniswapChainConfiguration chain,
        BigInteger fromBlock,
        BigInteger toBlock,
        [EnumeratorCancellation] CancellationToken ct = default)
    { 
        await foreach (var unichainLogsBatch in _unichainLogProvider.GetLogsAsync(chain, fromBlock, toBlock, ct))
        {
            var result = new List<LiquidityPoolPositionEvent>(unichainLogsBatch.Length);

            var web3 = _web3Factory.GetWeb3(chain);
            
            foreach (var log in unichainLogsBatch)
            {
                var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(log.TransactionHash);

                var tokenPair =
                    await _unichainLogReader.ReadTokenPairFromLogAsync(log.TransactionHash, receipt.Logs, ct);

                var @event =
                    _liquidityPoolEventDecoder.DecodeModifyLiquidityEvent(receipt.From.ToLowerInvariant(), log.Data,
                        tokenPair);

                result.Add(@event);
            }

            yield return result;
        }
    }
}