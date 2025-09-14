using CryptoWatcher.AaveModule.Services;
using CryptoWatcher.Abstractions;
using CryptoWatcher.Infrastructure.Aave.ExcelModels;
using CryptoWatcher.Infrastructure.Aave.Mappers;
using CryptoWatcher.Infrastructure.Excel;
using CryptoWatcher.Shared.Entities;
using SpreadCheetah.SourceGeneration;

namespace CryptoWatcher.Infrastructure.Aave;

public interface IAaveReportExcelService
{
    Task<Stream> CreateReportAsync(DateOnly? from, DateOnly? to, CancellationToken ct = default);
}

internal class AaveReportExcelService : BaseExcelReportService, IAaveReportExcelService
{
    private readonly IAaveReportService _aaveReportService;
    private readonly IRepository<Wallet> _walletRepository;

    public AaveReportExcelService(IAaveReportService aaveReportService, IRepository<Wallet> walletRepository)
    {
        _aaveReportService = aaveReportService;
        _walletRepository = walletRepository;
    }

    public async Task<Stream> CreateReportAsync(DateOnly? from, DateOnly? to, CancellationToken ct = default)
    {
        var (fromDate, toDate) = GetDefaultDatesIfNull(from, to);

        var wallets = await _walletRepository.ListAsync(ct);

        var ms = await CreateExcelWorkbookAsync(async workbook =>
        {
            foreach (var wallet in wallets)
            {
                var positionReports = await _aaveReportService.CreateReport(wallet, fromDate, toDate, ct);

                await workbook.StartWorksheetAsync(wallet.Address[..^32],
                    AaveExcelReportContext.Default.AavePositionExcelRow, ct);

                await workbook.AddHeaderRowAsync(AaveExcelReportContext.Default.AavePositionExcelRow, token: ct);

                foreach (var positionReport in positionReports)
                {
                    foreach (var report in positionReport.ReportItems)
                    {
                        var row = report.MapToExcelRow();

                        await workbook.AddAsRowAsync(row, AaveExcelReportContext.Default.AavePositionExcelRow, ct);
                    }

                    var totalRow = positionReport.MapToExcelModel(TotalName);

                    await workbook.AddAsRowAsync(totalRow, AaveExcelReportContext.Default.AavePositionExcelTotalRow,
                        ct);

                    await workbook.AddRowAsync([], ct);
                }
            }
        }, ct);
        
        return ms;
    }
}

[WorksheetRow(typeof(AavePositionExcelRow))]
[WorksheetRow(typeof(AavePositionExcelTotalRow))]
internal partial class AaveExcelReportContext : WorksheetRowContext;