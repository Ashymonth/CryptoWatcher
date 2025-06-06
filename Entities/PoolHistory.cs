namespace CryptoWatcher.Entities;

public class PoolHistory
{
    public DateOnly Day { get; set; }

    public decimal Token0Amount { get; set; }
    public decimal Token1Amount { get; set; }

    public decimal Token0AmountUsd { get; set; }
    public decimal Token1AmountUsd { get; set; }

    public decimal Token0FeesUnclaimed { get; set; }
    public decimal Token1FeesUnclaimed { get; set; }

    public decimal Token0FeesUnclaimedUsd { get; set; }
    public decimal Token1FeesUnclaimedUsd { get; set; }

    public decimal? Apr { get; set; }

    public bool IsActive { get; set; }

    public string NetworkName { get; set; } = null!;

    public Network Network { get; set; } = null!;
}