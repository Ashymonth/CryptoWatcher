using System.Numerics;

namespace CryptoWatcher.AaveModule.Models;

/// <summary>
/// Indicate that position is borrowed or supplied.
/// </summary>
public abstract class CalculatableAaveLendingPosition : AaveLendingPosition
{
    private const int RaiseToNormalize = 10;

    private const int ExponentToNormalize = 27;

    public required BigInteger ScaleAmount { get; init; }

    public required decimal TokenPriceInUsd { get; init; }

    protected abstract BigInteger AccrualIndex { get; }

    public BigInteger CalculateAmountWithInterest() =>
        ScaleAmount * AccrualIndex / BigInteger.Pow(RaiseToNormalize, ExponentToNormalize);
}