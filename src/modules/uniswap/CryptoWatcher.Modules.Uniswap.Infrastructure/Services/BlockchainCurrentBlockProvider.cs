using System.Numerics;
using CryptoWatcher.Modules.Uniswap.Abstractions;
using CryptoWatcher.Modules.Uniswap.Entities;

namespace CryptoWatcher.Modules.Uniswap.Infrastructure.Services;

public class BlockchainCurrentBlockProvider : IBlockchainCurrentBlockProvider
{
    private readonly IWeb3Factory _web3Factory;

    public BlockchainCurrentBlockProvider(IWeb3Factory web3Factory)
    {
        _web3Factory = web3Factory;
    }

    public async ValueTask<BigInteger> GetCurrentBlock(UniswapChainConfiguration chainConfiguration)
    {
        var web3 = _web3Factory.GetWeb3(chainConfiguration);

        var block = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();

        return block.Value;
    }
}