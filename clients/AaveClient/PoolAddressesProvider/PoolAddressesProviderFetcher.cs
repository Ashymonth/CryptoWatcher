using Nethereum.Web3;

namespace AaveClient.PoolAddressesProvider;

/// <summary>
/// Interface for fetching Aave Pool Addresses Provider-related information from a blockchain network.
/// Used for querying smart contracts to retrieve key details such as pool and price oracle addresses.
/// </summary>
public interface IPoolAddressesProviderFetcher
{
    /// <summary>
    /// Retrieves the pool address from the blockchain using the specified Pool Addresses Provider contract.
    /// </summary>
    /// <param name="blockchainUrl">The URL of the blockchain endpoint.</param>
    /// <param name="poolAddressesProviderAddress">The address of the Pool Addresses Provider contract.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the pool address as a string.</returns>
    Task<string> GetPoolAddressesAsync(string blockchainUrl, string poolAddressesProviderAddress);

    /// <summary>
    /// Retrieves the price oracle address from the blockchain using the specified Pool Addresses Provider contract.
    /// </summary>
    /// <param name="blockchainUrl">The URL of the blockchain endpoint.</param>
    /// <param name="poolAddressesProviderAddress">The address of the Pool Addresses Provider contract.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the price oracle address as a string.</returns>
    Task<string> GetPriceOracleAddressAsync(string blockchainUrl, string poolAddressesProviderAddress);
}

internal class PoolAddressesProviderFetcher : IPoolAddressesProviderFetcher
{
    public async Task<string> GetPoolAddressesAsync(string blockchainUrl, string poolAddressesProviderAddress)
    {
        return await CallFunctionAsync(blockchainUrl, poolAddressesProviderAddress, "getPoolAddresses");
    }

    public async Task<string> GetPriceOracleAddressAsync(string blockchainUrl, string poolAddressesProviderAddress)
    {
        return await CallFunctionAsync(blockchainUrl, poolAddressesProviderAddress, "getPriceOracle");
    }

    private static Task<string> CallFunctionAsync(string blockchainUrl, string poolAddressesProviderAddress,
        string functionName)
    {
        var web3 = new Web3(blockchainUrl);

        var service = web3.Eth.GetContract(PoolAddressesProviderAbi.Abi, poolAddressesProviderAddress);
        var function = service.GetFunction(functionName);

        return function.CallAsync<string>();
    }
}