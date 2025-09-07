using AaveClient.UiPoolDataProvider.Contracts;
using AaveClient.UiPoolDataProvider.Contracts.ReservesData;
using Nethereum.Contracts;
using Nethereum.Web3;

namespace AaveClient.UiPoolDataProvider;

/// <summary>
/// Interface for fetching data related to user reserves and reserves metadata
/// from the Aave protocol's UI pool data provider.
/// </summary>
public interface IUiPoolDataProviderFetcher
{
    /// <summary>
    /// Asynchronously retrieves data related to the user's reserve positions in a specific Aave network.
    /// </summary>
    /// <param name="mainnetAddress">The mainnet address for the specified Aave network.</param>
    /// <param name="smartContract">The network information, including addresses for the UI Pool Data Provider and Pool Addresses Provider.</param>
    /// <param name="userAddress">The address of the user whose reserve data is being queried.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a list of <c>UserReserveData</c> instances, each of which provides detailed information about the user's reserve positions.</returns>
    Task<List<UserReserveData>> GetUserReservesDataAsync(string mainnetAddress,
        AaveRegistry.SmartContractAddresses smartContract,
        string userAddress);

    /// <summary>
    /// Asynchronously retrieves detailed reserves data from the Aave protocol for a specific network.
    /// </summary>
    /// <param name="blockchainUrl">The blockchain URL to connect to the specified Aave network.</param>
    /// <param name="smartContract">The network information, including addresses for the UI Pool Data Provider and Pool Addresses Provider.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains an instance of <see cref="GetReservesDataOutput"/>>, which includes aggregated reserves data and base currency information for the Aave protocol on the specified network.</returns>
    Task<GetReservesDataOutput> GetReservesDataAsync(string blockchainUrl,
        AaveRegistry.SmartContractAddresses smartContract);
}

/// <summary>
/// <see cref="IUiPoolDataProviderFetcher"/>
/// </summary>
internal class UiPoolDataProviderFetcher : IUiPoolDataProviderFetcher
{
    public async Task<List<UserReserveData>> GetUserReservesDataAsync(
        string mainnetAddress,
        AaveRegistry.SmartContractAddresses smartContract,
        string userAddress)
    {
        var web3 = new Web3(mainnetAddress);

        var function = GetFunction(web3, "getUserReservesData", smartContract.UiPoolDataProviderAddress);

        return (await function.CallDeserializingToObjectAsync<UserReservesResponse>(
            smartContract.PoolAddressesProviderAddress,
            userAddress
        )).ReservesData;
    }

    public async Task<GetReservesDataOutput> GetReservesDataAsync(string blockchainUrl,
        AaveRegistry.SmartContractAddresses smartContract)
    {
        var web3 = new Web3(blockchainUrl);

        var function = GetFunction(web3, "getReservesData", smartContract.UiPoolDataProviderAddress);

        return await function.CallDeserializingToObjectAsync<GetReservesDataOutput>(smartContract
            .PoolAddressesProviderAddress);
    }

    private static Function GetFunction(IWeb3 web3, string functionName, string uiPoolDataProviderAddress)
    {
        var contract = web3.Eth.GetContract(UiPoolDataProviderFetcherAbi.Abi, uiPoolDataProviderAddress);

        return contract.GetFunction(functionName);
    }
}