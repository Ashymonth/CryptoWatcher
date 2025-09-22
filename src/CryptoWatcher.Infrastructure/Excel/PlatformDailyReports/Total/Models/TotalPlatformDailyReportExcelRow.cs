using CryptoWatcher.Shared.ValueObjects;
using SpreadCheetah.SourceGeneration;

namespace CryptoWatcher.Infrastructure.Excel.PlatformDailyReports.Total.Models;

public class TotalPlatformDailyReportExcelRow
{
    [ColumnHeader("Платформа")]
    [ColumnWidth(25)]
    public required string PlatformName { get; init; } = null!;
    
    [ColumnHeader("Начальная позиция в $")]
    [ColumnWidth(25)]
    [CellValueConverter(typeof(ValueObjectToExcelValueConverter<Money>))]
    [CellStyle(ExcelStyleRegistry.TwoDecimalPlaces)]
    public required Money InitialPositionInUsd { get; init; }

    [ColumnHeader("Текущая позиция в $")]
    [ColumnWidth(25)]
    [CellValueConverter(typeof(ValueObjectToExcelValueConverter<Money>))]
    [CellStyle(ExcelStyleRegistry.TwoDecimalPlaces)]
    public required Money CurrentPositionInUsd { get; init; }

    [ColumnHeader("Рост в $")]
    [ColumnWidth(25)]
    [CellValueConverter(typeof(ValueObjectToExcelValueConverter<Money>))]
    [CellStyle(ExcelStyleRegistry.TwoDecimalPlaces)]
    public Money ProfitInUsd => CurrentPositionInUsd - InitialPositionInUsd;

    [ColumnHeader("Рост в %")]
    [ColumnWidth(25)]
    [CellValueConverter(typeof(ValueObjectToExcelValueConverter<Percent>))]
    [CellStyle(ExcelStyleRegistry.Percent)]
    public Percent ProfitInPercent => CurrentPositionInUsd != 0
        ? (CurrentPositionInUsd - InitialPositionInUsd) / CurrentPositionInUsd
        : 0;
}