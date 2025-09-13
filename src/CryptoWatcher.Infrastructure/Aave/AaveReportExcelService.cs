using CryptoWatcher.AaveModule.Services;
using CryptoWatcher.Abstractions;
using CryptoWatcher.Infrastructure.Aave.ExcelModels;
using CryptoWatcher.Infrastructure.Aave.Mappers;
using CryptoWatcher.Infrastructure.Excel;
using CryptoWatcher.Shared.Entities;
using SpreadCheetah;
using SpreadCheetah.SourceGeneration;
using SpreadCheetah.Styling;

namespace CryptoWatcher.Infrastructure.Aave;

public class AaveReportExcelService
{
    private static readonly Dictionary<string, Style> StyleNameToStyleMap = new()
    {
        [ExcelStyleRegistry.Number] =
            new Style { Format = NumberFormat.Standard(StandardNumberFormat.NoDecimalPlaces) },
        [ExcelStyleRegistry.TwoDecimalPlaces] = new Style
            { Format = NumberFormat.Standard(StandardNumberFormat.TwoDecimalPlaces) },
        [ExcelStyleRegistry.Percent] = new Style { Format = NumberFormat.Standard(StandardNumberFormat.Percent) },
    };

    
    private readonly IAaveReportService _aaveReportService;
    private readonly IRepository<Wallet> _walletRepository;

    public AaveReportExcelService(IAaveReportService aaveReportService, IRepository<Wallet> walletRepository)
    {
        _aaveReportService = aaveReportService;
        _walletRepository = walletRepository;
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

        var wallets = await _walletRepository.ListAsync(ct);

        var ms = new MemoryStream();

        await using var workbook = await Spreadsheet.CreateNewAsync(ms, cancellationToken: ct);
        foreach (var (styleName, style) in StyleNameToStyleMap)
        {
            workbook.AddStyle(style, styleName);
        }
        
        foreach (var wallet in wallets)
        {
            var positionReports = await _aaveReportService.CreateReport(wallet, from.Value, to.Value, ct);

            await workbook.StartWorksheetAsync(wallet.Address[..16], token: ct);

            await workbook.AddHeaderRowAsync(AaveExcelReportContext.Default.AavePositionExcelRow, token: ct);
            
            foreach (var positionReport in positionReports)
            {
                foreach (var report in positionReport.ReportItems)
                {
                    var row = report.MapToExcelRow();

                    await workbook.AddAsRowAsync(row, AaveExcelReportContext.Default.AavePositionExcelRow, ct);
                }

                // var totalRow = positionReport.MapToExcelModel("Итого:");
                //
                // await workbook.AddAsRowAsync(totalRow, AaveExcelReportContext.Default.AavePositionExcelTotalRow, ct);
                //
                await workbook.AddRowAsync([], ct);
            }
        }

        await workbook.FinishAsync(ct);
        ms.Seek(0, SeekOrigin.Begin);
        File.Create("test.xlsx").Close();
        
        await File.WriteAllBytesAsync("test.xlsx", ms.ToArray());
        return ms;
    }
}

[WorksheetRow(typeof(AavePositionExcelRow))]
[WorksheetRow(typeof(AavePositionExcelTotalRow))]
internal partial class AaveExcelReportContext : WorksheetRowContext;