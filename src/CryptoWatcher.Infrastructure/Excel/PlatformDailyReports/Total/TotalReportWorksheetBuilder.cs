using CryptoWatcher.Infrastructure.Excel.PlatformDailyReports.Total.Models;
using CryptoWatcher.Models;
using CryptoWatcher.Shared.ValueObjects;
using SpreadCheetah;
using SpreadCheetah.SourceGeneration;

namespace CryptoWatcher.Infrastructure.Excel.PlatformDailyReports.Total;

internal interface ITotalReportWorksheetBuilder
{
    Task CreateTotalWorksheetAsync(Spreadsheet spreadsheet,
        IReadOnlyCollection<PlatformDailyReportData> platformDailyReports,
        CancellationToken ct = default);
}

internal class TotalReportWorksheetBuilder : ITotalReportWorksheetBuilder
{
    public async Task CreateTotalWorksheetAsync(Spreadsheet spreadsheet,
        IReadOnlyCollection<PlatformDailyReportData> platformDailyReports,
        CancellationToken ct = default)
    {
        var rowContext = TotalReportExcelRowContext.Default.TotalPlatformDailyReportExcelRow;
        await spreadsheet.StartWorksheetAsync("Все платформы", rowContext, ct);

        await spreadsheet.AddHeaderRowAsync(rowContext, token: ct);

        Money initialPositionInUsd = 0;
        Money currentPositionInUsd = 0;
        foreach (var platformDailyReportData in platformDailyReports)
        {
            var platformTotalRow = platformDailyReportData.Reports.Values.Select(pair =>
                    new TotalPlatformDailyReportExcelRow
                    {
                        PlatformName = platformDailyReportData.PlatformName,
                        InitialPositionInUsd = pair.Sum(report => report.PositionInUsd - report.ProfitInUsd),
                        CurrentPositionInUsd = pair.Sum(report => report.PositionInUsd)
                    })
                .First();

            await spreadsheet.AddAsRowAsync(platformTotalRow, rowContext, ct);

            initialPositionInUsd += platformTotalRow.InitialPositionInUsd;
            currentPositionInUsd += platformTotalRow.CurrentPositionInUsd;
        }

        var totalRow = new TotalPlatformDailyReportExcelTotalRow
        {
            TotalName = "Итого",
            InitialPositionInUsd = initialPositionInUsd,
            CurrentPositionInUsd = currentPositionInUsd
        };

        var totalRowContext = TotalReportExcelRowContext.Default.TotalPlatformDailyReportExcelTotalRow;
        await spreadsheet.AddAsRowAsync(totalRow, totalRowContext, ct);
    }
}

[WorksheetRow(typeof(TotalPlatformDailyReportExcelRow))]
[WorksheetRow(typeof(TotalPlatformDailyReportExcelTotalRow))]
internal partial class TotalReportExcelRowContext : WorksheetRowContext;

// internal class TotalReportSheetWriter : ExcelSheetDataWriter<TotalReportExcelRowContext, PlatformDailyReport,
//     PlatformDailyReportItem>
// {
//     protected override IReadOnlyCollection<PlatformDailyReportItem> GetReportItems(PlatformDailyReport report)
//     {
//         throw new NotImplementedException();
//     }
//
//     protected override Task WriteRowAsync(Spreadsheet workbook, PlatformDailyReportItem dailyReportItem,
//         CancellationToken ct)
//     {
//         throw new NotImplementedException();
//     }
//
//     protected override Task WriteTotalRowAsync(Spreadsheet workbook, PlatformDailyReport dailyReport,
//         CancellationToken ct)
//     {
//         throw new NotImplementedException();
//     }
// }