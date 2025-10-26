using CryptoWatcher.Exceptions;
using CryptoWatcher.Extensions;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Hyperliquid.Entities;

public class HyperliquidPositionDailyPerformance
{
    private HyperliquidPositionDailyPerformance()
    {
    }

    /// <summary>
    /// Gets the day associated with the performance of a specific Hyperliquid position.
    /// </summary>
    /// <remarks>
    /// This property represents the date for which the daily performance metrics of the position are calculated.
    /// It is initialized during the creation of the performance object and remains immutable afterward.
    /// </remarks>
    public DateOnly Day { get; private set; }

    /// <summary>
    /// Gets the blockchain address of the vault associated with the Hyperliquid position.
    /// </summary>
    /// <remarks>
    /// This property represents the unique identifier for the vault where the position's assets are held.
    /// It is set during the creation of the performance object and cannot be modified afterward.
    /// </remarks>
    public EvmAddress VaultAddress { get; private set; } = null!;

    /// <summary>
    /// Gets the blockchain wallet address associated with a specific Hyperliquid position.
    /// </summary>
    /// <remarks>
    /// This property represents the unique wallet identifier used within the blockchain network for the corresponding position.
    /// It is initialized during the creation of the position and remains immutable afterward.
    /// </remarks>
    public EvmAddress WalletAddress { get; private set; } = null!;

    /// <summary>
    /// Gets or sets the balance value in terms of USD for a specific position's daily performance.
    /// </summary>
    /// <remarks>
    /// The balance represents the current value of funds within the vault for a given day,
    /// expressed in USD. This property can be updated to reflect changes in the vault's daily balance.
    /// </remarks>
    public decimal BalanceInUsd { get; private set; }

    /// <summary>
    /// Gets the balance value for the position's daily performance in terms of USD.
    /// </summary>
    /// <remarks>
    /// The balance represents the calculated profit for a particular day by comparing
    /// snapshots of the vault position over a date range. It is computed in the
    /// <see cref="CreateFromSnapshot"/> method and cannot be modified externally.
    /// </remarks>
    public decimal ProfitInUsd { get; private set; }

    public static HyperliquidPositionDailyPerformance CreateFromSnapshot(
        HyperliquidVaultPosition position,
        HyperliquidVaultPositionSnapshot previousSnapshot,
        HyperliquidVaultPositionSnapshot currentSnapshot)
    {
        if (!previousSnapshot.VaultAddress.Equals(position.VaultAddress) ||
            !previousSnapshot.WalletAddress.Equals(position.WalletAddress))
        {
            throw new DomainException("Snapshots must belong to the same vault and wallet");
        }

        if (previousSnapshot.Day > currentSnapshot.Day)
        {
            throw new DomainException("Previous snapshot must be before current");
        }

        return new HyperliquidPositionDailyPerformance
        {
            Day = currentSnapshot.Day,
            VaultAddress = currentSnapshot.VaultAddress,
            WalletAddress = currentSnapshot.WalletAddress,
            BalanceInUsd = currentSnapshot.Balance,
            ProfitInUsd = position.CalculateProfitInUsd(previousSnapshot.Day, currentSnapshot.Day).Amount
        };
    }
}