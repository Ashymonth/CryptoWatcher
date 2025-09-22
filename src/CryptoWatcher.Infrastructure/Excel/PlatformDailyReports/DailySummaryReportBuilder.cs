using CryptoWatcher.Abstractions.Reports;
using CryptoWatcher.Infrastructure.Excel.PlatformDailyReports.Abstractions;
using CryptoWatcher.Infrastructure.Excel.PlatformDailyReports.Total;
using CryptoWatcher.Models;

namespace CryptoWatcher.Infrastructure.Excel.PlatformDailyReports;

internal class DailySummaryReportBuilder : BaseExcelReportService, IDailySummaryReportBuilder
{
    private readonly IExcelSheetBuilder[] _spreadCheetahSheetBuilders;
    private readonly ITotalReportWorksheetBuilder _totalReportWorksheetBuilder;

    public DailySummaryReportBuilder(IEnumerable<IExcelSheetBuilder> spreadCheetahSheetBuilders,
        ITotalReportWorksheetBuilder totalReportWorksheetBuilder)
    {
        _totalReportWorksheetBuilder = totalReportWorksheetBuilder;
        _spreadCheetahSheetBuilders = spreadCheetahSheetBuilders.ToArray();
    }

    public async Task<Stream> BuildReportAsync(IReadOnlyCollection<PlatformDailyReportData> reportsByPlatform,
        CancellationToken ct = default)
    {
        var result = await CreateExcelWorkbookAsync(async workbook =>
        {
            await _totalReportWorksheetBuilder.CreateTotalWorksheetAsync(workbook, reportsByPlatform, ct);
            
            foreach (var reportByPlatform in reportsByPlatform)
            {
                if (reportByPlatform.Reports.Count == 0)
                {
                    continue;
                }

                var report = reportByPlatform.Reports.First();
                if (report.Value.Count == 0)
                {
                    continue;
                }

                var reportItem = report.Value.First();

                var sheetBuilder = _spreadCheetahSheetBuilders.FirstOrDefault(builder =>
                    builder.CanProcess(reportItem));

                if (sheetBuilder is null)
                {
                    throw new InvalidOperationException(
                        $"For type: {reportItem.GetType().Name}  sheet builder was not found");
                }

                await sheetBuilder.CreateWorksheetAsync(workbook, reportByPlatform, ct);
            }
        }, ct);

        return result;
    }
}