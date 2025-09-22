using CryptoWatcher.Shared.ValueObjects;
using SpreadCheetah.SourceGeneration;

namespace CryptoWatcher.Infrastructure.Excel.PlatformDailyReports.Total.Models;

public class TotalPlatformDailyReportExcelTotalRow
{
    public required string TotalName { get; init; } = null!;
    
    [CellValueConverter(typeof(ValueObjectToExcelValueConverter<Money>))]
    [CellStyle(ExcelStyleRegistry.TwoDecimalPlaces)]
    public required Money InitialPositionInUsd { get; init; }

    [CellValueConverter(typeof(ValueObjectToExcelValueConverter<Money>))]
    [CellStyle(ExcelStyleRegistry.TwoDecimalPlaces)]
    public required Money CurrentPositionInUsd { get; init; }

    [CellValueConverter(typeof(ValueObjectToExcelValueConverter<Money>))]
    [CellStyle(ExcelStyleRegistry.TwoDecimalPlaces)]
    public Money ProfitInUsd => CurrentPositionInUsd - InitialPositionInUsd;

    [CellValueConverter(typeof(ValueObjectToExcelValueConverter<Percent>))]
    [CellStyle(ExcelStyleRegistry.Percent)]
    public Percent ProfitInPercent => CurrentPositionInUsd != 0
        ? (CurrentPositionInUsd - InitialPositionInUsd) / CurrentPositionInUsd
        : 0;
}