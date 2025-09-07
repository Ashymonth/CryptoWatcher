using AaveClient.PoolAddressesProvider;
using AaveClient.UiPoolDataProvider;

namespace AaveClient;

public interface IAaveApiClient
{
    IUiPoolDataProviderFetcher UiPoolDataProviderFetcher { get; }

    IPoolAddressesProviderFetcher PoolAddressesProviderFetcher { get; }
}

public class AaveApiClient : IAaveApiClient
{
    public AaveApiClient(IUiPoolDataProviderFetcher uiPoolDataProviderFetcher,
        IPoolAddressesProviderFetcher poolAddressesProviderFetcher)
    {
        UiPoolDataProviderFetcher = uiPoolDataProviderFetcher;
        PoolAddressesProviderFetcher = poolAddressesProviderFetcher;
    }

    public IUiPoolDataProviderFetcher UiPoolDataProviderFetcher { get; }

    public IPoolAddressesProviderFetcher PoolAddressesProviderFetcher { get; }
}