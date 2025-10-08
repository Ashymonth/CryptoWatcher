using System.Numerics;
using CryptoWatcher.Modules.Uniswap.Application.Models;
using CryptoWatcher.Modules.Uniswap.Entities;

namespace CryptoWatcher.Modules.Uniswap.Application.Abstractions;

public interface IBlockchainLogProvider
{
    Task<BlockchainLogBatch> GetLogsAsync(UniswapChainConfiguration chainConfiguration,
        BigInteger fromBlock,
        BigInteger toBlock);
}