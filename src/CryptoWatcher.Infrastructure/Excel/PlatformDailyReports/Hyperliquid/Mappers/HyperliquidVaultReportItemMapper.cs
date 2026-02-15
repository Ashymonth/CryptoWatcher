using CryptoWatcher.Infrastructure.Excel.PlatformDailyReports.Hyperliquid.Models;
using CryptoWatcher.Modules.Hyperliquid.Application.Features.Reports.Models;
using Riok.Mapperly.Abstractions;

namespace CryptoWatcher.Infrastructure.Excel.PlatformDailyReports.Hyperliquid.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
internal static partial class HyperliquidVaultReportItemMapper
{
    public static partial HyperliquidVaultPositionExcelRow MapToExcelModel(this HyperliquidVaultReportItem item);
}