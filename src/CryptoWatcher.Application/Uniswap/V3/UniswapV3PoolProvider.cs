using CryptoWatcher.Abstractions.Integrations;
using CryptoWatcher.Entities;
using CryptoWatcher.Models;
using Nethereum.Web3;
using UniswapClient.UniswapV3;

namespace CryptoWatcher.Application.Uniswap.V3;

public class UniswapV3PoolProvider : IUniswapPoolProvider<UniswapV3PositionInfo>
{
    private readonly UniswapV3PoolFactory _uniswapV3PoolFactory;
    private readonly UniswapV3LiquidityPool _pool;

    public UniswapV3PoolProvider(UniswapV3PoolFactory uniswapV3PoolFactory, UniswapV3LiquidityPool pool)
    {
        _uniswapV3PoolFactory = uniswapV3PoolFactory;
        _pool = pool;
    }
 
    public async Task<LiquidityPool> GetPoolAsync(IWeb3 web3, Network network, UniswapV3PositionInfo position)
    {
        var poolAddress = await _uniswapV3PoolFactory.GetPoolAddressAsync(web3,
            network.PoolFactoryAddress,
            position.Token0, position.Token1);

        return await _pool.GetPoolInfoAsync(web3, poolAddress, network.MultiCallAddress, position.TickLower,
            position.TickUpper);
    }
}