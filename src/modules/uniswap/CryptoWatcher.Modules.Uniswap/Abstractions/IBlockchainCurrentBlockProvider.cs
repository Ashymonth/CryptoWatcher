using System.Numerics;
using CryptoWatcher.Modules.Uniswap.Entities;

namespace CryptoWatcher.Modules.Uniswap.Abstractions;

public interface IBlockchainCurrentBlockProvider
{
    ValueTask<BigInteger> GetCurrentBlock(UniswapChainConfiguration chainConfiguration);
}