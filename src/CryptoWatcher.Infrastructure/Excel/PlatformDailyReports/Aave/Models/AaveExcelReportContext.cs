using SpreadCheetah.SourceGeneration;

namespace CryptoWatcher.Infrastructure.Excel.PlatformDailyReports.Aave.Models;

[WorksheetRow(typeof(AavePositionExcelRow))]
[WorksheetRow(typeof(AavePositionExcelTotalRow))]
internal partial class AaveExcelReportContext : WorksheetRowContext;