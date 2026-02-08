using CryptoWatcher.Abstractions.Reports;
using CryptoWatcher.Modules.Hyperliquid.Application.Features.Reports;
using CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization;
using CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization.Abstractions;
using CryptoWatcher.Modules.Hyperliquid.Infrastructure.Integrations.Hyperliquid.Api;
using CryptoWatcher.Modules.Hyperliquid.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace CryptoWatcher.Modules.Hyperliquid.Infrastructure.Extensions;

public static class HyperliquidModuleKeyedService
{
    public const string DailyPlatformKeyService = nameof(HyperliquidReportDataService);
}

public static class ServiceCollectionExtensions
{
    private const string BaseUrl = "https://api.hyperliquid.xyz";

    public static IServiceCollection AddHyperliquidModule(this IServiceCollection services,
        Func<IServiceProvider, Uri>? hyperliquidUriFactory = null)
    {
        services.AddKeyedScoped<IPlatformDailyReportDataProvider, HyperliquidReportDataService>(
            HyperliquidModuleKeyedService.DailyPlatformKeyService);

        services.AddRefitClient<IHyperliquidApi>()
            .ConfigureHttpClient((provider, client) =>
                client.BaseAddress = hyperliquidUriFactory?.Invoke(provider) ?? new Uri(BaseUrl));

        services.AddSingleton<IUnprocessedVaultUpdatesFilter, UnprocessedVaultUpdatesFilter>();
        services.AddSingleton<IHyperliquidVaultPositionUpdater, HyperliquidVaultPositionUpdater>();
        services.AddSingleton<IHyperliquidSnapshotsUpdater, HyperliquidSnapshotsUpdater>();
        services.AddSingleton<IHyperliquidGateway, HyperliquidApiGateway>();
        
        services.AddScoped<IHyperliquidVaultPositionSyncJob, HyperliquidVaultPositionSyncJob>();

        return services;
    }
}