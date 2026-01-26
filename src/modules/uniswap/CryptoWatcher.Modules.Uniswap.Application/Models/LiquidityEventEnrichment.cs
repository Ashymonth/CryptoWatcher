using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Uniswap.Application.Models;

public record LiquidityEventEnrichment
{
    public required DateTime TimeStamp { get; init; }

    public required TokenPair TokenPair { get; init; } = null!;
}