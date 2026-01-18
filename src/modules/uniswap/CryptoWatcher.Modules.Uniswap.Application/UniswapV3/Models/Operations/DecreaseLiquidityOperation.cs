using System.Numerics;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Uniswap.Application.UniswapV3.Models.Operations;

public class DecreaseLiquidityOperation : PositionOperation
{
    public Token Token0 { get; set; } = null!;
    
    public Token Token1 { get; set; } = null!;
    
    public BigInteger Commission0 { get; set; }
    
    public BigInteger Commission1 { get; set; }

    public bool IsPositionClosed { get; set; }
}