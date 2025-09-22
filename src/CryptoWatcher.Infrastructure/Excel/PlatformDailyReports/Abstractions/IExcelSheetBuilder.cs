using CryptoWatcher.Models;
using SpreadCheetah;

namespace CryptoWatcher.Infrastructure.Excel.PlatformDailyReports.Abstractions;

internal interface IExcelSheetBuilder
{
    bool CanProcess(PlatformDailyReport dailyReport);

    Task CreateWorksheetAsync(Spreadsheet workbook,
        PlatformDailyReportData platformDailyReportData,
        CancellationToken ct = default);
}