using CryptoWatcher.HyperliquidModule.Services;
using CryptoWatcher.Infrastructure.Excel;
using CryptoWatcher.Infrastructure.Hyperliquid.ExcelModels;
using CryptoWatcher.Infrastructure.Hyperliquid.Mappers;
using SpreadCheetah;
using SpreadCheetah.SourceGeneration;
using SpreadCheetah.Styling;

namespace CryptoWatcher.Infrastructure.Hyperliquid;

public class HyperliquidExcelService
{
    private const string ReportSheetName = "Hyperliquid";
    private const string TotalName = "Итого:";
    private const string EmptyValue = "-";

    private readonly IHyperliquidReportService _hyperliquidReportService;

    private static readonly Dictionary<string, Style> StyleNameToStyleMap = new()
    {
        [ExcelStyleRegistry.Number] =
            new Style { Format = NumberFormat.Standard(StandardNumberFormat.NoDecimalPlaces) },
        [ExcelStyleRegistry.TwoDecimalPlaces] = new Style
            { Format = NumberFormat.Standard(StandardNumberFormat.TwoDecimalPlaces) },
        [ExcelStyleRegistry.Percent] = new Style { Format = NumberFormat.Standard(StandardNumberFormat.Percent) },
    };

    public HyperliquidExcelService(IHyperliquidReportService hyperliquidReportService)
    {
        _hyperliquidReportService = hyperliquidReportService;
    }

    public async Task<Stream> CreateReportAsync(DateOnly? from, DateOnly? to, CancellationToken ct = default)
    {
        var now = DateTime.Now;
        var monthStart = new DateTime(now.Year, now.Month, 1);
        var monthEnd = monthStart.AddMonths(1).AddDays(-1);

        if (!from.HasValue || !to.HasValue)
        {
            from = DateOnly.FromDateTime(monthStart);
            to = DateOnly.FromDateTime(monthEnd);
        }

        var vaultReports = await _hyperliquidReportService.CreateReportAsync(from.Value, to.Value, ct);

        var ms = new MemoryStream();

        var sheet = await Spreadsheet.CreateNewAsync(ms, cancellationToken: ct);

        foreach (var (styleName, style) in StyleNameToStyleMap)
        {
            sheet.AddStyle(style, styleName);
        }

        await sheet.StartWorksheetAsync(ReportSheetName,
            HyperliquidVaultPositionExcelContext.Default.HyperliquidVaultPositionExcelRow, ct);

        await sheet.AddHeaderRowAsync(HyperliquidVaultPositionExcelContext.Default.HyperliquidVaultPositionExcelRow,
            token: ct);

        foreach (var vaultReport in vaultReports)
        {
            foreach (var vaultReportItem in vaultReport.ReportItems)
            {
                await sheet.AddAsRowAsync(vaultReportItem.MapToExcelModel(),
                    HyperliquidVaultPositionExcelContext.Default.HyperliquidVaultPositionExcelRow, ct);
            }

            await sheet.AddAsRowAsync(vaultReport.MapToExcelModel(TotalName, EmptyValue),
                HyperliquidVaultPositionExcelContext.Default.HyperliquidVaultPositionExcelTotalRow, ct);

            await sheet.AddRowAsync([], ct);
        }

        await sheet.FinishAsync(ct);
        ms.Seek(0, SeekOrigin.Begin);
        return ms;
    }
}

[WorksheetRow(typeof(HyperliquidVaultPositionExcelRow))]
[WorksheetRow(typeof(HyperliquidVaultPositionExcelTotalRow))]
internal partial class HyperliquidVaultPositionExcelContext : WorksheetRowContext;