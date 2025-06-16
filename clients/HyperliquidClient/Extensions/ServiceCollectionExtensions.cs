using HyperliquidClient.UserNonFundingLedgerUpdates;
using HyperliquidClient.UserVaultEquities;
using Microsoft.Extensions.DependencyInjection;

namespace HyperliquidClient.Extensions;

public static class ServiceCollectionExtensions
{
    private const string BaseUrl = "https://api.hyperliquid.xyz";

    public static void AddHyperLiquidClient(this IServiceCollection services,
        Func<IServiceProvider, Uri>? hyperliquidUriFactory = null)
    {
        services.AddHttpClient<UserNonFundingLedgerUpdatesClient>((provider, client) =>
        {
            client.BaseAddress = hyperliquidUriFactory?.Invoke(provider) ?? new Uri(BaseUrl);
        });

        services.AddHttpClient<UserVaultEquitiesClient>((provider, client) =>
        {
            client.BaseAddress = hyperliquidUriFactory?.Invoke(provider) ?? new Uri(BaseUrl);
        });

        services.AddScoped<IHyperliquidApiClient, HyperliquidApiClient>();
        services.AddScoped<HyperliquidApiClient>(provider =>
            (HyperliquidApiClient)provider.GetRequiredService<IHyperliquidApiClient>());
    }
}