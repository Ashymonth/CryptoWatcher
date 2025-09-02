using CryptoWatcher.HyperliquidModule.Models;
using CryptoWatcher.Infrastructure.Hyperliquid.ExcelModels;
using Riok.Mapperly.Abstractions;

namespace CryptoWatcher.Infrastructure.Hyperliquid.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
internal static partial class HyperliquidVaultReportItemMapper
{
    public static partial HyperliquidVaultPositionExcelRow MapToExcelModel(this HyperliquidVaultReportItem item);
}