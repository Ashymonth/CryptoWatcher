using CryptoWatcher.Shared.ValueObjects;

namespace CryptoWatcher.UniswapModule.Models;

public class UniswapPoolPositionReportItem
{
    public required string Network { get; init; } = null!;
    
    public required DateOnly Day { get; init; }

    public required Money PositionInUsd { get; init; }

    public required Money HoldInUsd { get; init; }

    public required Money FeeInUsd { get; init; }

    public required string TokenPairSymbols { get; init; } = null!;
}