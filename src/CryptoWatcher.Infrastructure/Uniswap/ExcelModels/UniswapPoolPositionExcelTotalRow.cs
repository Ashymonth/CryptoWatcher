using CryptoWatcher.Infrastructure.Excel;
using CryptoWatcher.Shared.ValueObjects;
using SpreadCheetah.SourceGeneration;

namespace CryptoWatcher.Infrastructure.Uniswap.ExcelModels;

internal class UniswapPoolPositionExcelTotalRow
{
    public required string? TotalName { get; init; }  

    [CellStyle(ExcelStyleRegistry.TwoDecimalPlaces)]
    [CellValueConverter(typeof(ValueObjectToExcelValueConverter<Money>))]
    public required Money TotalPositionInUsd { get; init; }

    [CellStyle(ExcelStyleRegistry.TwoDecimalPlaces)]
    [CellValueConverter(typeof(ValueObjectToExcelValueConverter<Money>))]
    public required Money TotalHoldInUsd { get; init; }

    [CellStyle(ExcelStyleRegistry.TwoDecimalPlaces)]
    [CellValueConverter(typeof(ValueObjectToExcelValueConverter<Money>))]
    public required Money TotalFeeInUsd { get; init; }

    [CellStyle(ExcelStyleRegistry.TwoDecimalPlaces)]
    [CellValueConverter(typeof(ValueObjectToExcelValueConverter<Money>))]
    public Money RoiNet => TotalPositionInUsd + TotalFeeInUsd - TotalHoldInUsd;
    
    [CellStyle(ExcelStyleRegistry.Percent)]
    [CellValueConverter(typeof(ValueObjectToExcelValueConverter<Percent>))]
    public Percent Apy => TotalFeeInUsd / TotalPositionInUsd * 12;
    
    public required string TokenPairSymbols { get; init; } = null!;

    public required string Network { get; init; } = null!;
}