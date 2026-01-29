using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace CryptoWatcher.Modules.Uniswap.Infrastructure.Integrations.Blockchain.UniswapV3.LiquidityPool.Contracts;

[FunctionOutput]
internal class TickInfo : IFunctionOutputDTO
{
    [Parameter("uint128", "liquidityGross", 1)]
    public BigInteger LiquidityGross { get; set; }

    [Parameter("int128", "liquidityNet", 2)]
    public BigInteger LiquidityNet { get; set; }

    [Parameter("uint256", "feeGrowthOutside0X128", 3)]
    public BigInteger FeeGrowthOutside0X128 { get; set; }

    [Parameter("uint256", "feeGrowthOutside1X128", 4)]
    public BigInteger FeeGrowthOutside1X128 { get; set; }

    [Parameter("int56", "tickCumulativeOutside", 5)]
    public BigInteger TickCumulativeOutside { get; set; }

    [Parameter("uint160", "secondsPerLiquidityOutsideX128", 6)]
    public BigInteger SecondsPerLiquidityOutsideX128 { get; set; }

    [Parameter("uint32", "secondsOutside", 7)]
    public uint SecondsOutside { get; set; }

    [Parameter("bool", "initialized", 8)]
    public bool Initialized { get; set; }
}