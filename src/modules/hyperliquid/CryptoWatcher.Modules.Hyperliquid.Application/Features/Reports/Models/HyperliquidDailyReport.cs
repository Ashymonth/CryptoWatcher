using CryptoWatcher.Models;

namespace CryptoWatcher.Modules.Hyperliquid.Application.Features.Reports.Models;

/// <summary>
/// Represents a report of a Hyperliquid vault, including financial metrics and detailed report items.
/// </summary>
public class HyperliquidDailyReport : PlatformDailyReport
{
    /// <summary>
    /// <summary>
    /// Gets the collection of detailed report items for the Hyperliquid vault,
    /// providing snapshot information such as balance and profits for each day.
    /// </summary>
    public required IReadOnlyCollection<HyperliquidVaultReportItem> ReportItems { get; init; } = [];

    public override string GetNeworkName()
    {
        // hyperliquid support only arbitrum network
        return "Arbitrum";
    }
}