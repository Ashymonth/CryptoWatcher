namespace CryptoWatcher.Shared.ValueObjects;

public readonly struct Percent
{
    public decimal Value { get; private init; }

    public static implicit operator Percent(decimal value)
    {
        return new Percent { Value = value };
    }
    
    public static implicit operator decimal(Percent value)
    {
        return value.Value;
    }
}