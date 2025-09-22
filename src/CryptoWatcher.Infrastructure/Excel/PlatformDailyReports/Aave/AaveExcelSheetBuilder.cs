using CryptoWatcher.AaveModule.Models;
using CryptoWatcher.Infrastructure.Excel.PlatformDailyReports.Aave.Models;
using CryptoWatcher.Infrastructure.Excel.PlatformDailyReports.Abstractions;
using CryptoWatcher.Models;
using SpreadCheetah;

namespace CryptoWatcher.Infrastructure.Excel.PlatformDailyReports.Aave;

internal class AaveExcelSheetBuilder : IExcelSheetBuilder
{
    private readonly AaveDailyReportExcelWorksheetWriter _worksheetWriter;

    public AaveExcelSheetBuilder(AaveDailyReportExcelWorksheetWriter worksheetWriter)
    {
        _worksheetWriter = worksheetWriter;
    }

    public bool CanProcess(PlatformDailyReport dailyReport) => dailyReport is AaveDailyReport;

    public async Task CreateWorksheetAsync(Spreadsheet workbook, PlatformDailyReportData platformDailyReportData,
        CancellationToken ct = default)
    {
        await _worksheetWriter.CreateWorksheetAsync(workbook,
            platformDailyReportData, AaveExcelReportContext.Default.AavePositionExcelRow, ct);
    }
}