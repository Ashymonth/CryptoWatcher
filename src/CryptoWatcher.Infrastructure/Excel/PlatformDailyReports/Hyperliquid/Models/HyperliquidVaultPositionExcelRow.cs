using CryptoWatcher.Shared.ValueObjects;
using SpreadCheetah.SourceGeneration;

namespace CryptoWatcher.Infrastructure.Excel.PlatformDailyReports.Hyperliquid.Models;

internal class HyperliquidVaultPositionExcelRow
{
    [ColumnHeader("Адрес хранилища")] public string VaultAddress { get; init; } = null!;
    [ColumnHeader("День")] 
    [ColumnWidth(15)]
    [CellValueConverter(typeof(DateOnlyExcelConverter))]
    public DateOnly Day { get; init; }  

    [ColumnHeader("Баланс")]
    [ColumnWidth(20)]
    [CellStyle(ExcelStyleRegistry.TwoDecimalPlaces)]
    [CellValueConverter(typeof(ValueObjectToExcelValueConverter<Money>))]
    public Money PositionInUsd { get; init; }

    [ColumnHeader("Изменение за день")]
    [ColumnWidth(20)]
    [CellStyle(ExcelStyleRegistry.TwoDecimalPlaces)]
    [CellValueConverter(typeof(ValueObjectToExcelValueConverter<Money>))]
    public Money DailyProfitInUsd { get; init; }

    [ColumnHeader("Изменение за день в процентах")]
    [ColumnWidth(30)]
    [CellStyle(ExcelStyleRegistry.Percent)]
    [CellValueConverter(typeof(ValueObjectToExcelValueConverter<Percent>))]
    public Percent DailyProfitInUsdPercent { get; init; }
}