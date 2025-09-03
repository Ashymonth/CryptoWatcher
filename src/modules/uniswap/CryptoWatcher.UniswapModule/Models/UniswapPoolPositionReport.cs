using CryptoWatcher.Shared.ValueObjects;

namespace CryptoWatcher.UniswapModule.Models;

public class UniswapPoolPositionReport
{
    public required Money TotalPositionInUsd { get; init; }

    public required Money TotalHoldInUsd { get; init; }

    public required Money TotalFeeInUsd { get; init; }

    public required IReadOnlyCollection<UniswapPoolPositionReportItem> ReportItems { get; init; } = [];
}