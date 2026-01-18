using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Uniswap.Application.UniswapV3.Models.Operations;

public class MintPositionOperation : PositionOperation
{
    public int TickLower { get; set; }

    public int TickUpper { get; set; }
    
    public Token Token0 { get; set; } = null!;

    public Token Token1 { get; set; } = null!;
}