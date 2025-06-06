using AaveClient.UiPoolDataProvider.Contracts;
using Nethereum.Web3;

namespace AaveClient.UiPoolDataProvider;

public interface IUiPoolDataProviderFetcher
{
    Task<List<UserReserveData>> GetUserReservesDataAsync(AaveNetwork network, string userAddress);
}

public class UiPoolDataProviderFetcher : IUiPoolDataProviderFetcher
{
    public async Task<List<UserReserveData>> GetUserReservesDataAsync(AaveNetwork network, string userAddress)
    {
        var networkInfo = NetworkRegistry.NetworkToRpcUrl[network];
        var web3 = new Web3(networkInfo.RpcAddress);

        var contract = web3.Eth.GetContract(UiPoolDataProviderFetcherAbi.Abi, networkInfo.PoolAddress);
        var function = contract.GetFunction("getUserReservesData");

        return (await function.CallDeserializingToObjectAsync<UserReservesResponse>(
            networkInfo.ProviderAddress,
            userAddress
        )).ReservesData;
    }
}