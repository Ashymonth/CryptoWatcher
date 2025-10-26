using CryptoWatcher.Extensions;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Aave.Entities;

public class AavePositionDailyPerformance
{
    private AavePositionDailyPerformance()
    {
    }

    /// <summary>
    /// Gets the unique identifier of the Aave position associated with this snapshot.
    /// </summary>
    /// <remarks>
    /// Serves as a reference to the specific Aave position that this snapshot is linked to.
    /// It is a part of the composite key (with the Day property) used to uniquely identify
    /// the snapshot and allows tracking of individual positions over time.
    /// </remarks>
    public Guid SnapshotPositionId { get; private set; }

    /// <summary>
    /// Represents the wallet address associated with the liquidity pool position.
    /// </summary>
    /// <remarks>
    /// This property holds the blockchain wallet address linked to the liquidity pool position.
    /// It is used to identify the owner of the position and manage the related account details.
    /// </remarks>
    public EvmAddress WalletAddress { get; private set; } = null!;

    /// <summary>
    /// Gets the specific day associated with the Aave position snapshot.
    /// </summary>
    /// <remarks>
    /// Represents the date for which this snapshot was taken. It is used as part of the
    /// composite key to uniquely identify the snapshot entry and can help in tracking
    /// changes or records daily.
    /// </remarks>
    public DateOnly Day { get; private set; }

    /// <summary>
    /// Gets or sets the name of the blockchain network associated with the Aave position.
    /// </summary>
    /// <remarks>
    /// Represents the specific network on which the Aave position exists, such as Ethereum or Polygon.
    /// This property is used to categorize and differentiate positions based on their network.
    /// </remarks>
    public string NetworkName { get; set; } = null!;

    /// <summary>
    /// Specifies the type of position in the Aave protocol, such as whether it represents a supply or borrow activity.
    /// </summary>
    /// <remarks>
    /// This property is used to indicate and distinguish the nature of the position,
    /// whether the wallet has supplied or borrowed tokens within the Aave ecosystem.
    /// It plays a critical role in classifying and managing position-related data.
    /// </remarks>
    public AavePositionType PositionType { get; private set; }

    /// <summary>
    /// Gets the symbol of the token associated with the Aave position.
    /// </summary>
    /// <remarks>
    /// Represents the ticker or abbreviated designation of the token. This value is used
    /// to identify the token across various financial data representations and calculations
    /// within the daily performance metrics of an Aave position.
    /// </remarks>
    public string TokenSymbol { get; private set; } = null!;

    /// <summary>
    /// Gets the monetary representation of the position's value in USD.
    /// </summary>
    /// <remarks>
    /// Reflects the equivalent value of the position in United States Dollars
    /// based on the associated token's amount and its price in USD. This property
    /// provides a standardized way to evaluate the position's worth in fiat currency.
    /// </remarks>
    public decimal PositionInUsd { get; private set; }

    /// <summary>
    /// Represents the quantity of the token held in the Aave position for a given snapshot.
    /// </summary>
    /// <remarks>
    /// Reflects the token amount at the specific time captured by the snapshot.
    /// This value is critical for calculating performance metrics, such as changes in token holdings and profits,
    /// and provides a foundation for analyzing changes in value and trading decisions.
    /// </remarks>
    public decimal PositionInToken { get; private set; }

    /// <summary>
    /// Represents the profit, in USD, derived from a specific Aave position over the course of a day.
    /// </summary>
    /// <remarks>
    /// This property quantifies the change in the value of a position (measured in USD) on a given day,
    /// factoring in market price fluctuations, accrued interest, and other relevant metrics.
    /// It is calculated by comparing the USD value of the position across two snapshots
    /// and may either be positive (indicating gains) or negative (indicating losses).
    /// </remarks>
    public decimal ProfitInUsd { get; private set; }

    /// <summary>
    /// Gets the profit calculated in terms of the token's amount for the associated Aave position on a specific day.
    /// </summary>
    /// <remarks>
    /// Represents the net gain or loss in the token's quantity for a given day, as a result of changes in the Aave position.
    /// Useful for tracking performance in token units over time.
    /// </remarks>
    public decimal ProfitInToken { get; private set; }

    public static AavePositionDailyPerformance Create(AavePosition position, AavePositionSnapshot previous,
        AavePositionSnapshot current)
    {
        var profitInUsd = position.CalculateProfitInUsd(previous.Day, current.Day);
        var profitInToken = position.CalculateProfitInToken(previous.Day, current.Day);

        return new AavePositionDailyPerformance
        {
            Day = current.Day,
            WalletAddress = position.WalletAddress,
            NetworkName = position.Network,
            PositionType = position.PositionType,
            TokenSymbol = current.Token.Symbol,
            PositionInUsd = current.Token.AmountInUsd,
            ProfitInUsd = profitInUsd.Amount,
            PositionInToken = current.Token.Amount,
            ProfitInToken = profitInToken.Amount,
            SnapshotPositionId = current.PositionId
        };
    }
}