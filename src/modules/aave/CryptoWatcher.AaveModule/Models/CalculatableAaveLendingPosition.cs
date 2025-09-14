using System.Numerics;
using CryptoWatcher.AaveModule.Entities;
using CryptoWatcher.Extensions;

namespace CryptoWatcher.AaveModule.Models;

/// <summary>
/// Indicate that position is borrowed or supplied.
/// </summary>
public abstract class CalculatableAaveLendingPosition : AaveLendingPosition
{
    private const int AaveIndexDecimals = 27;
    
    private static readonly BigInteger AaveIndexNormalizationFactor = BigInteger.Pow(10, AaveIndexDecimals);
     
    public required BigInteger ScaleAmount { get; init; }
    
    public required decimal TokenPriceInUsd { get; init; }
    
    public required byte TokenDecimals { get; init; }
    
    protected abstract BigInteger AccrualIndex { get; }

    public decimal CalculatePositionScaleInToken() => ScaleAmount.ToDecimal(TokenDecimals);
    
    public BigInteger CalculateAmountWithInterest() => ScaleAmount * AccrualIndex / AaveIndexNormalizationFactor;

    public AavePositionType DeterminePositionType() => this switch
    {
        BorrowedAaveLendingPosition _ => AavePositionType.Borrowed,
        SuppliedAaveLendingPosition _ => AavePositionType.Supplied,
        _ => throw new InvalidOperationException()
    };
}