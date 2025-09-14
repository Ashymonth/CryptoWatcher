using AaveClient.PoolAddressesProvider;
using AaveClient.UiPoolDataProvider;
using Microsoft.Extensions.DependencyInjection;

namespace AaveClient.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddAaveClient(this IServiceCollection services)
    {
        services.AddSingleton<IUiPoolDataProviderFetcher, UiPoolDataProviderFetcher>();
        services.AddSingleton<IPoolAddressesProviderFetcher, PoolAddressesProviderFetcher>();

        services.AddSingleton<IAaveApiClient, AaveApiClient>();
    }
}