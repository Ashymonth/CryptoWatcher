using AaveClient.UiPoolDataProvider;

namespace AaveClient;

public interface IAaveApiClient
{
    IUiPoolDataProviderFetcher UiPoolDataProviderFetcher { get; }
}

public class AaveApiClient : IAaveApiClient
{
    public AaveApiClient(IUiPoolDataProviderFetcher uiPoolDataProviderFetcher)
    {
        UiPoolDataProviderFetcher = uiPoolDataProviderFetcher;
    }

    public IUiPoolDataProviderFetcher  UiPoolDataProviderFetcher { get;  }
}