using CryptoWatcher.Application;
using CryptoWatcher.Infrastructure.Services;
using CryptoWatcher.UniswapModule.Abstractions;
using CryptoWatcher.UniswapModule.Specifications;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoWatcher.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUniswapPositionsSyncService, UniswapPositionsSyncService>();
        
        services.AddScoped<ITokenEnricher, TokenEnricher>();
        services.AddScoped<TokenService>();
        services.AddScoped<CoinNormalizer>();
        return services;
    }
}