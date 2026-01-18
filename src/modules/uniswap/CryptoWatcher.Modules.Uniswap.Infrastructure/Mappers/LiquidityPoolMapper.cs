using CryptoWatcher.Modules.Uniswap.Infrastructure.Client.Models;
using CryptoWatcher.Modules.Uniswap.Models;
using Riok.Mapperly.Abstractions;

namespace CryptoWatcher.Modules.Uniswap.Infrastructure.Mappers;

[Mapper]
internal static partial class LiquidityPoolMapper
{
    public static partial LiquidityPool MapToLiquidityPool(this LiquidityPoolInfo poolInfo);
}