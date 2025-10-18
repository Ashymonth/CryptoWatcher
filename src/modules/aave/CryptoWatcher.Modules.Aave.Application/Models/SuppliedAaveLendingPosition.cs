using System.Numerics;
using CryptoWatcher.Modules.Aave.Application.Models;

namespace CryptoWatcher.Modules.Aave.Models;

/// <summary>
/// Indicates that the position is supplied.
/// </summary>
public class SuppliedAaveLendingPosition : CalculatableAaveLendingPosition
{
    /// <summary>
    /// For positions with supplied liquidity, this property represents the liquidity index.
    /// </summary>
    public required BigInteger LiquidityIndex { get; init; }

    protected override BigInteger AccrualIndex => LiquidityIndex;
}