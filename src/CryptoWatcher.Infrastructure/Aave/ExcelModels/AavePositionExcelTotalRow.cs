using CryptoWatcher.Infrastructure.Excel;
using CryptoWatcher.Shared.ValueObjects;
using SpreadCheetah.SourceGeneration;

namespace CryptoWatcher.Infrastructure.Aave.ExcelModels;

internal class AavePositionExcelTotalRow
{
    public required string TotalName { get; init; }

    [CellValueConverter(typeof(ValueObjectToExcelValueConverter<Money>))]
    [CellStyle(ExcelStyleRegistry.TwoDecimalPlaces)]
    public required Money Balance { get; init; }

    public required string TokenSymbol { get; init; } = null!;

    [CellValueConverter(typeof(ValueObjectToExcelValueConverter<Money>))]
    [CellStyle(ExcelStyleRegistry.TwoDecimalPlaces)]
    public required Money TotalProfitInUsd { get; init; }
    
    public required decimal TotalProfitInToken { get; init; }
}