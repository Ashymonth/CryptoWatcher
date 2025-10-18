using CryptoWatcher.Modules.Aave.Entities;
using CryptoWatcher.Modules.Aave.Models;
using CryptoWatcher.Shared.Entities;

namespace CryptoWatcher.Modules.Aave.Abstractions;

/// <summary>
/// Defines methods for interacting with the Aave protocol, allowing retrieval of lending positions and other related data.
/// </summary>
public interface IAaveProvider
{
    /// <summary>
    /// Retrieves the list of lending positions from the Aave protocol for a specified network and wallet.
    /// </summary>
    /// <param name="chain"></param>
    /// <param name="wallet">The wallet whose lending positions will be queried.</param>
    /// <param name="ct">Optional cancellation token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of <see cref="AaveLendingPosition"/> objects.</returns>
    Task<List<AaveLendingPosition>> GetLendingPositionAsync(AaveChainConfiguration chain, Wallet wallet,
        CancellationToken ct = default);
}