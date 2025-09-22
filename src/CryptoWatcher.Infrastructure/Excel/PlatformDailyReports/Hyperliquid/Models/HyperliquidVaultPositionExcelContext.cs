using SpreadCheetah.SourceGeneration;

namespace CryptoWatcher.Infrastructure.Excel.PlatformDailyReports.Hyperliquid.Models;

[WorksheetRow(typeof(HyperliquidVaultPositionExcelRow))]
[WorksheetRow(typeof(HyperliquidVaultPositionExcelTotalRow))]
internal partial class HyperliquidVaultPositionExcelContext : WorksheetRowContext;