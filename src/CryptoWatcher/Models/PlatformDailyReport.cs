using CryptoWatcher.Shared.ValueObjects;

namespace CryptoWatcher.Models;

public abstract class PlatformDailyReport
{
    public required Money PositionInUsd { get; init; }

    public required Money ProfitInUsd { get; init; }

    public required Percent ProfitInPercent { get; init; }
}

public class PlatformDailyReportData<TReportItem> : PlatformDailyReport
    where TReportItem : PlatformDailyReportItem
{
    protected IReadOnlyCollection<PlatformDailyReportItem> ReportItems { get; init; }
}