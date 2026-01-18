using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Uniswap.Application.UniswapV3.Models.Operations;

public class CollectFeesOperation : PositionOperation
{
   public Token Token0 { get; set; } = null!;

   public Token Token1 { get; set; } = null!;
}