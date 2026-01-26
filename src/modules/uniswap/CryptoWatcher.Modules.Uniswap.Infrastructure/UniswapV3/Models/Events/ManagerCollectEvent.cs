using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace CryptoWatcher.Modules.Uniswap.Infrastructure.UniswapV3.Models.Events;

[Event("Collect")]
public class ManagerCollectEvent
{
    [Parameter("uint256", "tokenId", 1, true)]
    public BigInteger TokenId { get; set; }

    [Parameter("address", "recipient", 2, false)]
    public string Recipient { get; set; } = null!;

    [Parameter("uint256", "amount0", 3, false)]
    public BigInteger Amount0 { get; set; }

    [Parameter("uint256", "amount1", 4, false)]
    public BigInteger Amount1 { get; set; }
}