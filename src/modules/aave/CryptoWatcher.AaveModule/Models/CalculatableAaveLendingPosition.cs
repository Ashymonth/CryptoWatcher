using System.Numerics;
using CryptoWatcher.AaveModule.Entities;

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

    public AavePositionType DeterminePositionType() => this switch
    {
        BorrowedAaveLendingPosition _ => AavePositionType.Borrowed,
        SuppliedAaveLendingPosition _ => AavePositionType.Supplied,
        _ => throw new InvalidOperationException()
    };
}