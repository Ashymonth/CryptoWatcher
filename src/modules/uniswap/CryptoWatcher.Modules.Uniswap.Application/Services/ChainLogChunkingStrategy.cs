using System.Numerics;
using CryptoWatcher.Modules.Uniswap.Abstractions;

namespace CryptoWatcher.Modules.Uniswap.Application.Services;

public class ChainLogChunkingStrategy : IChainLogChunkingStrategy
{
    private static readonly BigInteger ChunkSize = new(1000);

    public IEnumerable<(BigInteger from, BigInteger to)> CreateChunks(BigInteger fromBlock, BigInteger toBlock)
    {
        var currentChunkStart = fromBlock;

        while (currentChunkStart <= toBlock)
        {
            var chunkEnd = currentChunkStart + ChunkSize - 1;
            if (chunkEnd > toBlock)
            {
                chunkEnd = toBlock;
            }
            
            yield return (currentChunkStart, chunkEnd);
            
            currentChunkStart = chunkEnd + 1;
        }
    }
}