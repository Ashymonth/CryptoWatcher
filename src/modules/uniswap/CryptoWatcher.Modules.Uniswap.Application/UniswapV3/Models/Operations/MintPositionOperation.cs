using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Uniswap.Application.UniswapV3.Models.Operations;

public class MintPositionOperation : PositionOperation
{
    public EvmAddress PollAddress { get; set; } = null!;
    
    public int TickLower { get; init; }

    public int TickUpper { get; init; }
}