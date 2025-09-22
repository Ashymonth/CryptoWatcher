using CryptoWatcher.UniswapModule.Models;
using Riok.Mapperly.Abstractions;
using UniswapClient.Models;

namespace CryptoWatcher.Infrastructure.Excel.PlatformDailyReports.Uniswap.Mappers;

[Mapper]
internal static partial class LiquidityPoolMapper
{
    public static partial LiquidityPool MapToLiquidityPool(this LiquidityPoolInfo poolInfo);
}