 
using CryptoWatcher.Modules.Aave.Application.Models;
using CryptoWatcher.Modules.Aave.Entities;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Aave.Infrastructure.Client.UiPoolDataProvider;

/// <summary>
/// Interface for fetching data related to user reserves and reserves metadata
/// from the Aave protocol's UI pool data provider.
/// </summary>
public interface IUiPoolDataProviderFetcher
{
    /// <summary>
    /// Asynchronously retrieves data related to the user's reserve positions in a specific Aave network.
    /// </summary>
    /// <param name="chain">The network information, including addresses for the UI Pool Data Provider and Pool Addresses Provider.</param>
    /// <param name="userAddress">The address of the user whose reserve data is being queried.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a list of <c>UserReserveData</c> instances, each of which provides detailed information about the user's reserve positions.</returns>
    Task<IReadOnlyCollection<UserReserve>> GetUserReservesDataAsync(AaveChainConfiguration chain,
        EvmAddress userAddress);

    /// <summary>
    /// Asynchronously retrieves detailed reserves data from the Aave protocol for a specific network.
    /// </summary>
    Task<MarketReserveOutput> GetMarketReservesDataAsync(AaveChainConfiguration chain);
}