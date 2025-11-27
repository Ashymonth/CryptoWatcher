using CryptoWatcher.Shared.ValueObjects;
using SpreadCheetah.SourceGeneration;

namespace CryptoWatcher.Infrastructure.Excel.PlatformDailyReports.Aave.Models;

internal class AavePositionExcelRow
{
    [ColumnHeader("День")]
    [ColumnWidth(15)]
    [CellValueConverter(typeof(DateOnlyExcelConverter))]
    public required DateOnly Day { get; init; }

    [ColumnHeader("Токен")] 
    [ColumnWidth(10)]
    public required string TokenSymbol { get; init; } = null!;
    
    [ColumnHeader("Сеть")] 
    [ColumnWidth(10)]
    public required string NetworkName { get; init; } = null!;
    
    [ColumnHeader("Позиция в $")]
    [ColumnWidth(20)]
    [CellValueConverter(typeof(ValueObjectToExcelValueConverter<Money>))]
    [CellStyle(ExcelStyleRegistry.TwoDecimalPlaces)]
    public required Money PositionInUsd { get; init; }
    
    [ColumnHeader("Позиция в токене")]
    [ColumnWidth(25)]
    public required decimal PositionInToken { get; init; }
    
    [ColumnHeader("Рост позиции в $")]
    [ColumnWidth(20)]
    [CellValueConverter(typeof(ValueObjectToExcelValueConverter<Money>))]
    [CellStyle(ExcelStyleRegistry.TwoDecimalPlaces)]
    public required Money PositionGrowInUsd { get; init; }
    
    [ColumnHeader("Доход за день в $")]
    [ColumnWidth(20)]
    [CellValueConverter(typeof(ValueObjectToExcelValueConverter<Money>))]
    [CellStyle(ExcelStyleRegistry.TwoDecimalPlaces)]
    public required Money DailyProfitInUsd { get; init; }
    
    [ColumnHeader("Доход за день в токене")]
    [ColumnWidth(25)]
    public required decimal DailyProfitInToken { get; init; }
    
    [ColumnHeader("Кэш флоу $")]
    [ColumnWidth(25)]
    public required decimal CashFlowsInUsd { get; init; } 
    
    [ColumnHeader("Кэш флоу токен")]
    [ColumnWidth(25)]
    public required decimal CashFlowsInToken { get; init; } 
}