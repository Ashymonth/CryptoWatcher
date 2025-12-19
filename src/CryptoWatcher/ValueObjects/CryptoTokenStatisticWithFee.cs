namespace CryptoWatcher.ValueObjects;

public record CryptoTokenStatisticWithFee : CryptoTokenStatistic
{
    public required decimal Fee { get; init; }

    public decimal FeeInUsd => Fee * PriceInUsd;
}