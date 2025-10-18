using CryptoWatcher.Modules.Aave.Application.Abstractions.Client;
using CryptoWatcher.Modules.Aave.Infrastructure.Client.UiPoolDataProvider;

namespace CryptoWatcher.Modules.Aave.Infrastructure.Client;

public class AaveApiClient : IAaveApiClient
{
    public AaveApiClient(IUiPoolDataProviderFetcher uiPoolDataProviderFetcher)
    {
        UiPoolDataProviderFetcher = uiPoolDataProviderFetcher;
    }

    public IUiPoolDataProviderFetcher UiPoolDataProviderFetcher { get; }
}