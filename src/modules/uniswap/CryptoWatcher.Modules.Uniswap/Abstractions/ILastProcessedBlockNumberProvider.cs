using System.Numerics;

namespace CryptoWatcher.Modules.Uniswap.Abstractions;

public interface ILastProcessedBlockNumberProvider
{
    ValueTask<BigInteger> GetLastProcessedBlockNumberAsync(CancellationToken ct = default);
}