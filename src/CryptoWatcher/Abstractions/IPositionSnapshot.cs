namespace CryptoWatcher.Abstractions;

public interface IPositionSnapshot
{
    DateOnly Day { get; init; }
}