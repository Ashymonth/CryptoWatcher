using CryptoWatcher.Shared.ValueObjects;

namespace CryptoWatcher.Modules.Uniswap.Application.Models;

public class LiquidityEventEnrichment
{
    public required DateTimeOffset TimeStamp { get; init; }

    public required TokenPair TokenPair { get; init; } = null!;
}