using CryptoWatcher.Shared.ValueObjects;

namespace CryptoWatcher.AaveModule.Models;

/// <summary>
/// Represents a report for a position in Aave, including balance, token details, and daily-profit metrics.
/// </summary>
public class AavePositionReport
{
    public string TokenSymbol => ReportItems?.FirstOrDefault()?.Position.Symbol ?? string.Empty;

    public Money Balance => ReportItems.LastOrDefault()?.Position.AmountInUsd ?? 0;

    public Money TotalCommissionInUsd => ReportItems.Sum(item => item.CommissionInUsd);

    public decimal TotalCommissionInToken => ReportItems.Sum(item => item.CommissionInToken);

    public IReadOnlyCollection<AavePositionReportItem> ReportItems { get; init; } = [];
}