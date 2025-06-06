using CryptoWatcher.Abstractions.Integrations;
using CryptoWatcher.Entities;
using CryptoWatcher.Host.Services.Uniswap.V4;
using CryptoWatcher.Models;
using Nethereum.Web3;
using UniswapClient.UniswapV4;

namespace CryptoWatcher.Application.Uniswap.V4;

public class UniswapV4PoolProvider : IUniswapPoolProvider<UniswapV4PositionInfo>
{
    private readonly UniswapV4StateView _stateView;

    public UniswapV4PoolProvider(UniswapV4StateView stateView)
    {
        _stateView = stateView;
    }
    
    public async Task<LiquidityPool> GetPoolAsync(IWeb3 web3, Network network, UniswapV4PositionInfo position)
    {
        var sot0 = await _stateView.GetSlot0Async(web3, position.PoolKey);

        var tickLower = await _stateView.GetTickInfoAsync(web3, position.PoolKey, position.TickLower);

        var tickUpper = await _stateView.GetTickInfoAsync(web3, position.PoolKey, position.TickUpper);

        var feeGlobal = await _stateView.GetFeeGrowGlobalAsync(web3, position.PoolKey);

        return new LiquidityPool
        {
            SqrtPriceX96 = sot0.SqrtPriceX96,
            Tick = sot0.Tick,
            FeeGrowthGlobal0X128 = feeGlobal.FeeGrowthGlobal0,
            FeeGrowthGlobal1X128 = feeGlobal.FeeGrowthGlobal1,
            LowerTick = new LiquidityPoolTick
            {
                FeeGrowthOutside0X128 = tickLower.FeeGrowthOutside0X128,
                FeeGrowthOutside1X128 = tickLower.FeeGrowthOutside1X128
            },
            UpperTick = new LiquidityPoolTick
            {
                FeeGrowthOutside0X128 = tickUpper.FeeGrowthOutside0X128,
                FeeGrowthOutside1X128 = tickUpper.FeeGrowthOutside1X128
            }
        };
    }
}