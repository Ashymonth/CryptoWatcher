using System.Numerics;
using CryptoWatcher.Modules.Uniswap.Abstractions;

namespace CryptoWatcher.Modules.Uniswap.Application.Services;

public class LastProcessedBlockNumberProvider : ILastProcessedBlockNumberProvider
{
    public ValueTask<BigInteger> GetLastProcessedBlockNumberAsync(CancellationToken ct = default)
    {
        return ValueTask.FromResult<BigInteger>(28813747);
    }
}