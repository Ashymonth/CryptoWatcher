using System.Numerics;
using System.Runtime.CompilerServices;
using CryptoWatcher.Modules.Uniswap.Entities;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;

namespace CryptoWatcher.Modules.Uniswap.Infrastructure.Services;

internal interface IUnichainLogProvider
{
    IAsyncEnumerable<FilterLog[]> GetLogsAsync(UniswapChainConfiguration chainConfiguration, BigInteger fromBlock,
        BigInteger toBlock,
        CancellationToken ct = default);
}

internal class UnichainLogProvider : IUnichainLogProvider
{
    private static readonly BigInteger ChunkSize = new(1000);
    private readonly IWeb3Factory  _web3Factory;

    public UnichainLogProvider(IWeb3Factory web3Factory)
    {
        _web3Factory = web3Factory;
    }

    public async IAsyncEnumerable<FilterLog[]> GetLogsAsync(UniswapChainConfiguration chainConfiguration,
        BigInteger fromBlock,
        BigInteger toBlock, [EnumeratorCancellation] CancellationToken ct = default)
    {
        var web3 = _web3Factory.GetWeb3(chainConfiguration);
        
        var currentChunkStart = fromBlock;

        while (currentChunkStart <= toBlock)
        {
            ct.ThrowIfCancellationRequested();

            var chunkEnd = currentChunkStart + ChunkSize - 1;
            if (chunkEnd > toBlock)
            {
                chunkEnd = toBlock;
            }

            var filter = new NewFilterInput
            {
                FromBlock = new BlockParameter(currentChunkStart.ToHexBigInteger()),
                ToBlock = new BlockParameter(chunkEnd.ToHexBigInteger()),
                Topics =
                [
                    new[] { UnichainWellKnownField.V4ModifyLiquiditySignature },
                    null,
                    new[] { UnichainWellKnownField.V4PositionManagerAddress }
                ],
            };

            var logs = await web3.Eth.Filters.GetLogs.SendRequestAsync(filter);
            yield return logs;

            currentChunkStart = chunkEnd + 1;
        }
    }
}