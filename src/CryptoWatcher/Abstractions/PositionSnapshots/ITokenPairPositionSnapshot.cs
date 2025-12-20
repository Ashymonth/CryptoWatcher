using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Abstractions.PositionSnapshots;

public interface ITokenPairPositionSnapshot : IPositionSnapshot
{
    CryptoTokenStatisticWithFee Token0 { get; }
    
    CryptoTokenStatisticWithFee Token1 { get; }
}