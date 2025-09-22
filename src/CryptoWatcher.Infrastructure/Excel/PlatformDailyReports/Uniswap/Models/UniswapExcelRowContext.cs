using SpreadCheetah.SourceGeneration;

namespace CryptoWatcher.Infrastructure.Excel.PlatformDailyReports.Uniswap.Models;

[WorksheetRow(typeof(UniswapPoolPositionExcelRow))]
[WorksheetRow(typeof(UniswapPoolPositionExcelTotalRow))]
internal partial class UniswapExcelRowContext : WorksheetRowContext;