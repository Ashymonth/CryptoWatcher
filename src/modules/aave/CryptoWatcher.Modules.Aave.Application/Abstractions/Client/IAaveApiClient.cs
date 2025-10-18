using CryptoWatcher.Modules.Aave.Infrastructure.Client.UiPoolDataProvider;

namespace CryptoWatcher.Modules.Aave.Application.Abstractions.Client;

public interface IAaveApiClient
{
    IUiPoolDataProviderFetcher UiPoolDataProviderFetcher { get; }
}