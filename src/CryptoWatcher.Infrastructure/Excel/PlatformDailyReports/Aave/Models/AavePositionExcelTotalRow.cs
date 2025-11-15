using CryptoWatcher.Shared.ValueObjects;
using SpreadCheetah.SourceGeneration;

namespace CryptoWatcher.Infrastructure.Excel.PlatformDailyReports.Aave.Models;

internal class AavePositionExcelTotalRow
{
    public required string TotalName { get; init; }

    public required string TokenSymbol { get; init; } = null!;
    
    public required string NetworkName { get; init; } = null!;
    
    [CellValueConverter(typeof(ValueObjectToExcelValueConverter<Money>))]
    [CellStyle(ExcelStyleRegistry.TwoDecimalPlaces)]
    public required Money PositionInUsd { get; init; }
    
    public required decimal PositionInToken { get; init; }
    
    [CellValueConverter(typeof(ValueObjectToExcelValueConverter<Money>))]
    [CellStyle(ExcelStyleRegistry.TwoDecimalPlaces)]
    public required Money PositionGrowInUsd { get; init; }
    
    [CellValueConverter(typeof(ValueObjectToExcelValueConverter<Money>))]
    [CellStyle(ExcelStyleRegistry.TwoDecimalPlaces)]
    public required Money ProfitInUsd { get; init; }
    
    public required decimal ProfitInToken { get; init; }
}