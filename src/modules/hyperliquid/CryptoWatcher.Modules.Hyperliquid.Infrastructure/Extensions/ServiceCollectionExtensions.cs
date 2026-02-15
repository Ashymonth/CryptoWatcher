using System.Threading.RateLimiting;
using CryptoWatcher.Abstractions.Reports;
using CryptoWatcher.Modules.Hyperliquid.Application.Features.Reports;
using CryptoWatcher.Modules.Hyperliquid.Application.Features.Reports.Abstractions;
using CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization;
using CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization.Abstractions;
using CryptoWatcher.Modules.Hyperliquid.Infrastructure.Integrations.Hyperliquid;
using CryptoWatcher.Modules.Hyperliquid.Infrastructure.Integrations.Hyperliquid.Api;
using CryptoWatcher.Modules.Hyperliquid.Infrastructure.Persistence;
using CryptoWatcher.Modules.Hyperliquid.Infrastructure.Persistence.Queries;
using CryptoWatcher.Modules.Hyperliquid.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Polly;
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
        string connectionString,
        Func<IServiceProvider, Uri>? hyperliquidUriFactory = null)
    {
        services.AddDbContext<HyperliquidDbContext>(options =>
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "hyperliquid");
                npgsql.MigrationsAssembly(typeof(HyperliquidDbContext).Assembly.FullName);
            }));
        
        services.AddKeyedScoped<IPlatformDailyReportDataProvider, HyperliquidReportDataService>(
            HyperliquidModuleKeyedService.DailyPlatformKeyService);

        services.AddRefitClient<IHyperliquidApi>()
            .ConfigureHttpClient((provider, client) =>
                client.BaseAddress = hyperliquidUriFactory?.Invoke(provider) ?? new Uri(BaseUrl))
            .AddResilienceHandler("rate-limiter", builder =>
            {
                //https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/rate-limits-and-user-limits
                builder.AddRateLimiter(new SlidingWindowRateLimiter(
                    new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = 50,
                        Window = TimeSpan.FromMinutes(1),
                        SegmentsPerWindow = 6,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 10
                    }));
            });

        services.AddScoped<IHyperliquidSyncStateRepository, HyperliquidSyncStateRepository>();
        services.AddScoped<IHyperliquidVaultPositionRepository, HyperliquidVaultPositionRepository>();
        services.AddScoped<HyperliquidVaultPositionBulkWriter>();
        services.AddScoped<IHyperliquidPositionForReportQuery, HyperliquidPositionForReportQuery>();

        services.AddSingleton<IUnprocessedVaultUpdatesFilter, UnprocessedVaultUpdatesFilter>();
        services.AddSingleton<IHyperliquidVaultPositionUpdater, HyperliquidVaultPositionUpdater>();
        services.AddSingleton<IHyperliquidSnapshotsUpdater, HyperliquidSnapshotsUpdater>();
        services.AddSingleton<IHyperliquidGateway, HyperliquidApiGateway>();

        services.AddScoped<IHyperliquidVaultPositionSyncJob, HyperliquidVaultPositionSyncJob>();

        return services;
    }
}