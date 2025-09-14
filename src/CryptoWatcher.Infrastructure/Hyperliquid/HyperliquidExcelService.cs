using CryptoWatcher.HyperliquidModule.Services;
using CryptoWatcher.Infrastructure.Excel;
using CryptoWatcher.Infrastructure.Hyperliquid.ExcelModels;
using CryptoWatcher.Infrastructure.Hyperliquid.Mappers;
using SpreadCheetah.SourceGeneration;

namespace CryptoWatcher.Infrastructure.Hyperliquid;

/// <summary>
/// Defines methods for generating Excel reports based on Hyperliquid data.
/// </summary>
public interface IHyperliquidExcelService
{
    /// <summary>
    /// Generates a report based on the specified date range and returns it as a stream.
    /// </summary>
    /// <param name="from">The starting date for the report. If null, the default start date is the first day of the current month.</param>
    /// <param name="to">The ending date for the report. If null, the default end date is the last day of the current month.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A stream containing the generated report in Excel format.</returns>
    Task<Stream> CreateReportAsync(DateOnly? from, DateOnly? to, CancellationToken ct = default);
}

internal class HyperliquidExcelService : BaseExcelReportService, IHyperliquidExcelService
{
    private const string ReportSheetName = "Hyperliquid";
    private const string EmptyValue = "-";

    private readonly IHyperliquidReportService _hyperliquidReportService;

    public HyperliquidExcelService(IHyperliquidReportService hyperliquidReportService)
    {
        _hyperliquidReportService = hyperliquidReportService;
    }

    public async Task<Stream> CreateReportAsync(DateOnly? from, DateOnly? to, CancellationToken ct = default)
    {
        var (fromDate, toDate) = GetDefaultDatesIfNull(from, to);

        var vaultReports = await _hyperliquidReportService.CreateReportAsync(fromDate, toDate, ct);

        var ms = await CreateExcelWorkbookAsync(async sheet =>
        {
            var rowContext = HyperliquidVaultPositionExcelContext.Default.HyperliquidVaultPositionExcelRow;
            var totalContext = HyperliquidVaultPositionExcelContext.Default.HyperliquidVaultPositionExcelTotalRow;
            
            await sheet.StartWorksheetAsync(ReportSheetName, rowContext, ct);

            await sheet.AddHeaderRowAsync(HyperliquidVaultPositionExcelContext.Default.HyperliquidVaultPositionExcelRow,
                token: ct);

            foreach (var vaultReport in vaultReports)
            {
                foreach (var vaultReportItem in vaultReport.ReportItems)
                {
                    await sheet.AddAsRowAsync(vaultReportItem.MapToExcelModel(), rowContext, ct);
                }

                await sheet.AddAsRowAsync(vaultReport.MapToExcelModel(TotalName, EmptyValue), totalContext, ct);

                await sheet.AddRowAsync([], ct);
            }
        }, ct);

        return ms;
    }
}

[WorksheetRow(typeof(HyperliquidVaultPositionExcelRow))]
[WorksheetRow(typeof(HyperliquidVaultPositionExcelTotalRow))]
internal partial class HyperliquidVaultPositionExcelContext : WorksheetRowContext;