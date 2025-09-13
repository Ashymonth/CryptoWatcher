using CryptoWatcher.Shared.ValueObjects;

namespace CryptoWatcher.AaveModule.Models;

public class AavePositionReportItem
{
    /// <summary>
    /// Gets the day for which the Aave position report is generated.
    /// </summary>
    /// <remarks>
    /// This property indicates the specific calendar day associated with the report,
    /// providing context for the recorded balance, profit, and performance metrics.
    /// </remarks>
    public required DateOnly Day { get; init; }

    /// <summary>
    /// Gets or sets the detailed information about the token associated with the Aave position.
    /// </summary>
    /// <remarks>
    /// This property encapsulates the token's details, including its symbol, amount, and valuation in USD,
    /// providing comprehensive context about the specific asset in the position.
    /// </remarks>
    public required TokenInfo Position { get; init; }

    public required Money PositionChange { get; init; }

    public required decimal CommissionInToken { get; init; }

    public Money CommissionInUsd => CommissionInToken * Position.PriceInUsd; 
}