using CryptoWatcher.Infrastructure.Excel;
using CryptoWatcher.Infrastructure.Uniswap.ExcelModels;
using CryptoWatcher.Infrastructure.Uniswap.Mappers;
using CryptoWatcher.UniswapModule.Services;
using SpreadCheetah.SourceGeneration;

namespace CryptoWatcher.Infrastructure.Uniswap;

public interface IUniswapExcelReportService
{
    Task<Stream> ExportPoolInfoToExcelAsync(DateOnly? from, DateOnly? to, CancellationToken ct = default);
}

internal class UniswapExcelReportService : BaseExcelReportService, IUniswapExcelReportService
{
    private const string ReportName = "Uniswap";

    private readonly IUniswapReportService _uniswapReportService;

    public UniswapExcelReportService(IUniswapReportService uniswapReportService)
    {
        _uniswapReportService = uniswapReportService;
    }

    public async Task<Stream> ExportPoolInfoToExcelAsync(DateOnly? from, DateOnly? to, CancellationToken ct = default)
    {
        var (fromDate, toDate) = GetDefaultDatesIfNull(from, to);

        var poolPositions = await _uniswapReportService.CreateReportAsync(fromDate, toDate, ct);

        var ms = await CreateExcelWorkbookAsync(async sheet =>
        {
            var rowContext = PoolInfoExcelRowContext.Default.UniswapPoolPositionExcelRow;
            var totalContext = PoolInfoExcelRowContext.Default.UniswapPoolPositionExcelTotalRow;
            await sheet.StartWorksheetAsync(ReportName, rowContext, ct);

            await sheet.AddHeaderRowAsync(rowContext, token: ct);

            foreach (var poolPosition in poolPositions)
            {
                foreach (var positionSnapshot in poolPosition.ReportItems)
                {
                    var excelRow = positionSnapshot.MapToExcelRowModel();

                    await sheet.AddAsRowAsync(excelRow, rowContext, ct);
                }

                if (poolPosition.ReportItems.Count != 0)
                {
                    // pool can contain data only for 1 network and 1 token pair, 
                    // so we can take just the first item
                    var item = poolPosition.ReportItems.First();
                    var totalExcelRow = poolPosition.MapToExcelModel(TotalName, item.TokenPairSymbols, item.Network);

                    await sheet.AddAsRowAsync(totalExcelRow, totalContext, ct);
                }

                await sheet.AddRowAsync([], ct);
            }
        }, ct);


        return ms;
    }
}

[WorksheetRow(typeof(UniswapPoolPositionExcelRow))]
[WorksheetRow(typeof(UniswapPoolPositionExcelTotalRow))]
internal partial class PoolInfoExcelRowContext : WorksheetRowContext;