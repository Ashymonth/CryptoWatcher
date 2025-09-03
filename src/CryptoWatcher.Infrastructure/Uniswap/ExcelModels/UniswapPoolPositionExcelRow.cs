using CryptoWatcher.Infrastructure.Excel;
using CryptoWatcher.Shared.ValueObjects;
using SpreadCheetah.SourceGeneration;

namespace CryptoWatcher.Infrastructure.Uniswap.ExcelModels;

internal class UniswapPoolPositionExcelRow
{
    [ColumnHeader("День")]
    [CellValueConverter(typeof(DateOnlyExcelConverter))]
    public required DateOnly Day { get; init; }  

    [ColumnHeader("Позиция в $")]
    [ColumnWidth(20)]
    [CellStyle(ExcelStyleRegistry.TwoDecimalPlaces)]
    [CellValueConverter(typeof(ValueObjectToExcelValueConverter<Money>))]
    public required Money PositionInUsd { get; init; }

    [ColumnHeader("HOLD $")]
    [ColumnWidth(20)]
    [CellStyle(ExcelStyleRegistry.TwoDecimalPlaces)]
    [CellValueConverter(typeof(ValueObjectToExcelValueConverter<Money>))]
    public required Money HoldInUsd { get; init; }

    [ColumnHeader("Комиссия в $")]
    [ColumnWidth(20)]
    [CellStyle(ExcelStyleRegistry.TwoDecimalPlaces)]
    [CellValueConverter(typeof(ValueObjectToExcelValueConverter<Money>))]
    public required Money FeeInUsd { get; init; }

    [ColumnHeader("Прибыль")]
    [ColumnWidth(20)]
    [CellStyle(ExcelStyleRegistry.TwoDecimalPlaces)]
    [CellValueConverter(typeof(ValueObjectToExcelValueConverter<Money>))]
    public Money RoiNet => Math.Round(PositionInUsd + FeeInUsd - HoldInUsd, 2);

    [ColumnHeader("APY %")]
    [CellStyle(ExcelStyleRegistry.Percent)]
    [CellValueConverter(typeof(ValueObjectToExcelValueConverter<Percent>))]
    public Percent Apy => FeeInUsd / PositionInUsd * 12;

    [ColumnHeader("Пара")] public required string TokenPairSymbols { get; init; } = null!;

    [ColumnHeader("Сеть")] public required string Network { get; init; } = null!;
}