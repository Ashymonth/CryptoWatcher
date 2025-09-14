namespace CryptoWatcher.Shared.ValueObjects;

public readonly struct Money
{
    public decimal Value { get; private init; }

    public static implicit operator Money(decimal value)
    {
        return new Money { Value = value };
    }
    
    public static implicit operator decimal(Money value)
    {
        return value.Value;
    }
}