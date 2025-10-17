using CryptoWatcher.Modules.Aave.Infrastructure.Client.UiPoolDataProvider;

namespace CryptoWatcher.Modules.Aave.Infrastructure.Client;

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

    public IUiPoolDataProviderFetcher UiPoolDataProviderFetcher { get; }
}