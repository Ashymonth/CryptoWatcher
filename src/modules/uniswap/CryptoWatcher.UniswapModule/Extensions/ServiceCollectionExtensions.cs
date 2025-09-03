using CryptoWatcher.UniswapModule.Services;
using CryptoWatcher.UniswapModule.Specifications;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoWatcher.UniswapModule.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUniswapModule(this IServiceCollection services)
    {
        services.AddSingleton<IUniswapMath, UniswapMath>();
        services.AddScoped<IUniswapReportService, UniswapReportService>();
        services.AddScoped<IUniswapPositionsSyncService, UniswapPositionsSyncService>();

        return services;
    }
}