using CryptoWatcher.Modules.Uniswap.Entities;
using CryptoWatcher.Modules.Uniswap.Infrastructure.Client.Models;
using CryptoWatcher.Modules.Uniswap.Infrastructure.Client.UniswapV4.StateView;
using Nethereum.Web3;

namespace CryptoWatcher.Modules.Uniswap.Infrastructure.Client.UniswapV4.LiquidityPool;

internal interface IUniswapV4LiquidityPool
{
    Task<LiquidityPoolInfo> GetPoolAsync(UniswapChainConfiguration chain, UniswapV4PositionInfo position);
}

internal class UniswapV4LiquidityPool : IUniswapV4LiquidityPool
{
    private readonly IUniswapV4StateView _stateView;

    public UniswapV4LiquidityPool(IUniswapV4StateView stateView)
    {
        _stateView = stateView;
    }

    public async Task<LiquidityPoolInfo> GetPoolAsync(UniswapChainConfiguration chain,  UniswapV4PositionInfo position)
    {
        var sot0 = await _stateView.GetSlot0Async(chain, position.PoolKey);

        var tickLower = await _stateView.GetTickInfoAsync(chain, position.PoolKey, position.TickLower);

        var tickUpper = await _stateView.GetTickInfoAsync(chain, position.PoolKey, position.TickUpper);

        var feeGlobal = await _stateView.GetFeeGrowGlobalAsync(chain, position.PoolKey);

        return new LiquidityPoolInfo
        {
            SqrtPriceX96 = sot0.SqrtPriceX96,
            Tick = sot0.Tick,
            FeeGrowthGlobal0X128 = feeGlobal.FeeGrowthGlobal0,
            FeeGrowthGlobal1X128 = feeGlobal.FeeGrowthGlobal1,
            LowerTick = new PoolTickInfo
            {
                FeeGrowthOutside0X128 = tickLower.FeeGrowthOutside0X128,
                FeeGrowthOutside1X128 = tickLower.FeeGrowthOutside1X128
            },
            UpperTick = new PoolTickInfo
            {
                FeeGrowthOutside0X128 = tickUpper.FeeGrowthOutside0X128,
                FeeGrowthOutside1X128 = tickUpper.FeeGrowthOutside1X128
            }
        };
    }
}