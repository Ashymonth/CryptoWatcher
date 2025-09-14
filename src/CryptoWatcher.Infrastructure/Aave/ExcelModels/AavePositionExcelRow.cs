using CryptoWatcher.Infrastructure.Excel;
using CryptoWatcher.Shared.ValueObjects;
using SpreadCheetah.SourceGeneration;

namespace CryptoWatcher.Infrastructure.Aave.ExcelModels;

internal class AavePositionExcelRow
{
    [ColumnHeader("День")]
    [CellValueConverter(typeof(DateOnlyExcelConverter))]
    public required DateOnly Day { get; init; }

    [ColumnHeader("Токен")] public required string TokenSymbol { get; init; } = null!;
    
    [ColumnHeader("Позиция в $")]
    [CellValueConverter(typeof(ValueObjectToExcelValueConverter<Money>))]
    [CellStyle(ExcelStyleRegistry.TwoDecimalPlaces)]
    public required Money PositionInUsd { get; init; }
    
    [ColumnHeader("Динамика в $")]
    [CellValueConverter(typeof(ValueObjectToExcelValueConverter<Money>))]
    [CellStyle(ExcelStyleRegistry.TwoDecimalPlaces)]
    public required Money PositionChange { get; init; }
    
    [ColumnHeader("Комиссия в $")]
    [CellValueConverter(typeof(ValueObjectToExcelValueConverter<Money>))]
    [CellStyle(ExcelStyleRegistry.TwoDecimalPlaces)]
    public required Money CommissionInUsd { get; init; }
    
    [ColumnHeader("Комиссия в $/%")]
    [CellValueConverter(typeof(ValueObjectToExcelValueConverter<Percent>))]
    [CellStyle(ExcelStyleRegistry.TwoDecimalPlaces)]
    public required Percent CommissionInUsdPercent { get; init; }

    [ColumnHeader("Комиссия в токена")]
    public required decimal CommissionInToken { get; init; }
    
    [ColumnHeader("Комиссия в $/%")]
    [CellValueConverter(typeof(ValueObjectToExcelValueConverter<Percent>))]
    [CellStyle(ExcelStyleRegistry.Percent)]
    public required Percent CommissionInTokenPercent { get; init; }
}