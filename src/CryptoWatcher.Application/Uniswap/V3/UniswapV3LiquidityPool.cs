using System.Numerics;
using CryptoWatcher.Extensions;
using CryptoWatcher.Models;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Web3;

namespace CryptoWatcher.Application.Uniswap.V3;

public class UniswapV3LiquidityPool
{
    public async Task<LiquidityPool> GetPoolInfoAsync(IWeb3 web3, string poolAddress, string multiCallAddress,
        int tickLower, int tickUpper)
    {
        // 1. Подготавливаем все вызовы
        var calls = new List<Call>
        {
            new()
            {
                Target = poolAddress,
                CallData = new Slot0Function().GetCallData()
            },
            new()
            {
                Target = poolAddress,
                CallData = new FeeGrowthGlobal0X128Function().GetCallData()
            },
            new()
            {
                Target = poolAddress,
                CallData = new FeeGrowthGlobal1X128Function().GetCallData()
            },
            new()
            {
                Target = poolAddress,
                CallData = new TicksFunction { Tick = tickLower }.GetCallData()
            },
            new()
            {
                Target = poolAddress,
                CallData = new TicksFunction { Tick = tickUpper }.GetCallData()
            }
        };

        var result = await web3.MultiCallAsync(calls, multiCallAddress);

        var slot0 = new Slot0OutputDto().DecodeOutput(result[0].ToHex());
        var feeGrowthGlobal0X128 = new FeeGrowthGlobalOutputDTO().DecodeOutput(result[1].ToHex());
        var feeGrowthGlobal1X128 = new FeeGrowthGlobalOutputDTO().DecodeOutput(result[2].ToHex());
        var tickLowerData = new TickInfo().DecodeOutput(result[3].ToHex());
        var tickUpperData = new TickInfo().DecodeOutput(result[4].ToHex());
        
        return new LiquidityPool
        {
            Tick = slot0.Tick,
            SqrtPriceX96 = slot0.SqrtPriceX96,
            FeeGrowthGlobal0X128 = feeGrowthGlobal0X128.Value,
            FeeGrowthGlobal1X128 = feeGrowthGlobal1X128.Value,
            LowerTick = new LiquidityPoolTick
            {
                FeeGrowthOutside0X128 = tickLowerData.FeeGrowthOutside0X128,
                FeeGrowthOutside1X128 = tickLowerData.FeeGrowthOutside1X128
            },
            UpperTick = new LiquidityPoolTick
            {
                FeeGrowthOutside0X128 = tickUpperData.FeeGrowthOutside0X128,
                FeeGrowthOutside1X128 = tickUpperData.FeeGrowthOutside1X128
            }
        };
    }

    // Необходимые FunctionMessage классы
    [Function("slot0", typeof(Slot0OutputDto))]
    private class Slot0Function : FunctionMessage
    {
    }

    [Function("feeGrowthGlobal0X128", "uint256")]
    private class FeeGrowthGlobal0X128Function : FunctionMessage
    {
    }

    [Function("feeGrowthGlobal1X128", "uint256")]
    private class FeeGrowthGlobal1X128Function : FunctionMessage
    {
    }

    [Function("ticks", typeof(TickInfo))]
    private class TicksFunction : FunctionMessage
    {
        [Parameter("int24", "tick")] public int Tick { get; set; }
    }

    [FunctionOutput]
    private class FeeGrowthGlobalOutputDTO : IFunctionOutputDTO
    {
        [Parameter("uint256", "", 1)] public BigInteger Value { get; set; }
    }

    [FunctionOutput]
    private class Slot0OutputDto : IFunctionOutputDTO
    {
        [Parameter("uint160", "sqrtPriceX96", 1)]
        public BigInteger SqrtPriceX96 { get; set; }

        [Parameter("int24", "tick", 2)] public int Tick { get; set; }
    }

    [FunctionOutput]
    private class TickInfo : IFunctionOutputDTO
    {
        [Parameter("uint128", "liquidityGross", 1)]
        public BigInteger LiquidityGross { get; set; }

        [Parameter("int128", "liquidityNet", 2)]
        public BigInteger LiquidityNet { get; set; }

        [Parameter("int128", "stakedLiquidityNet", 3)]
        public BigInteger StakedLiquidityNet { get; set; }

        [Parameter("uint256", "feeGrowthOutside0X128", 4)]
        public BigInteger FeeGrowthOutside0X128 { get; set; }

        [Parameter("uint256", "feeGrowthOutside1X128", 5)]
        public BigInteger FeeGrowthOutside1X128 { get; set; }

        [Parameter("uint256", "rewardGrowthOutsideX128", 6)]
        public BigInteger RewardGrowthOutsideX128 { get; set; }

        [Parameter("int56", "tickCumulativeOutside", 7)]
        public BigInteger TickCumulativeOutside { get; set; }

        [Parameter("uint160", "secondsPerLiquidityOutsideX128", 8)]
        public BigInteger SecondsPerLiquidityOutsideX128 { get; set; }

        [Parameter("uint32", "secondsOutside", 9)]
        public uint SecondsOutside { get; set; }

        [Parameter("bool", "initialized", 10)] public bool Initialized { get; set; }
    }
}