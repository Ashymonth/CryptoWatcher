using System.Numerics;

namespace CryptoWatcher.Modules.Uniswap.Application.Services.PoisitionEventsSync.UniswapV3.Models.Operations;

public class DecreaseLiquidityOperation : PositionOperation
{
    public BigInteger Commission0 { get; init; }
    
    public BigInteger Commission1 { get; init; }

    public bool IsPositionClosed { get; init; }
}