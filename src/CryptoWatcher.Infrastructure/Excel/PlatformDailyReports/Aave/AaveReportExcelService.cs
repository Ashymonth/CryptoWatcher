using CryptoWatcher.AaveModule.Extensions;
using CryptoWatcher.Abstractions.Reports;
using CryptoWatcher.Infrastructure.Excel.PlatformDailyReports.Aave.Models;
using CryptoWatcher.Shared.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoWatcher.Infrastructure.Excel.PlatformDailyReports.Aave;

public interface IAaveReportExcelService
{
    Task<Stream> CreateReportAsync(IReadOnlyCollection<Wallet> wallets, DateOnly? from, DateOnly? to,
        CancellationToken ct = default);
}

internal class AaveReportExcelService : BaseExcelReportService, IAaveReportExcelService
{
    private readonly IPlatformDailyReportDataProvider _platformDailyReportDataProvider;
    private readonly AaveDailyReportExcelWorksheetWriter _worksheetWriter;

    public AaveReportExcelService(
        [FromKeyedServices(AaveModuleKeyedService.DailyPlatformKeyService)]
        IPlatformDailyReportDataProvider platformDailyReportDataProvider,
        AaveDailyReportExcelWorksheetWriter worksheetWriter)
    {
        _platformDailyReportDataProvider = platformDailyReportDataProvider;
        _worksheetWriter = worksheetWriter;
    }

    public async Task<Stream> CreateReportAsync(IReadOnlyCollection<Wallet> wallets, DateOnly? from, DateOnly? to,
        CancellationToken ct = default)
    {
        var (fromDate, toDate) = GetDefaultDatesIfNull(from, to);

        var reportData = await _platformDailyReportDataProvider.GetReportDataAsync(wallets, fromDate, toDate, ct);

        var rowContext = AaveExcelReportContext.Default.AavePositionExcelRow;
        var ms = await CreateExcelWorkbookAsync(_worksheetWriter, rowContext, reportData, ct);

        return ms;
    }
}