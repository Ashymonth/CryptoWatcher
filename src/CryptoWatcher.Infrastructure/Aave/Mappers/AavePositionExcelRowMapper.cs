using CryptoWatcher.AaveModule.Models;
using CryptoWatcher.Infrastructure.Aave.ExcelModels;
using Riok.Mapperly.Abstractions;

namespace CryptoWatcher.Infrastructure.Aave.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
internal static partial class AavePositionExcelRowMapper
{
    [MapProperty(nameof(@AavePositionReportItem.Position.AmountInUsd), nameof(AavePositionExcelRow.PositionInUsd))]
    [MapProperty(nameof(@AavePositionReportItem.Position.Symbol), nameof(AavePositionExcelRow.TokenSymbol))]
    public static partial AavePositionExcelRow MapToExcelRow(this AavePositionReportItem item);
}