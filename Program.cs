using CryptoWatcher.Entities;
using CryptoWatcher.Models;
using CryptoWatcher.Services;
using Nethereum.Web3;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHybridCache();
builder.Services.AddHttpClient<CoinGeckoTokenPriceProvider>(client => client.BaseAddress = new Uri("https://api.coingecko.com"));

builder.Services.AddSingleton<TokenService>();
builder.Services.AddSingleton<TokenEnricher>();
builder.Services.AddScoped<LiquidityPoolService>();
builder.Services.AddScoped<PoolFactoryService>();
builder.Services.AddScoped<UniswapV3PoolManager>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
});
// [
//     
//     new RedisConfiguration
//     {
//         ConnectionString = "localhost:6379",
//         // ConnectionString =
//         //     "31.207.64.66:6379,password=B5TiQ9QQfXgajmeT,abortConnect=false"
//     }
// ]);
//

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var fetcher =
        new UniswapV3PositionFetcher("https://mainnet.era.zksync.io", "0x7581a80c84d7488be276e6c7b4c1206f25946502");
    var poolFactory = scope.ServiceProvider.GetRequiredService<TokenEnricher>();
    
    var networkConstants = new Network("https://mainnet.era.zksync.io",
        "0x7581a80c84d7488be276e6c7b4c1206f25946502",
        "0x9D63d318143cF14FF05f8AAA7491904A494e6f13",
        "0xb1f9b5fcd56122cdfd7086e017ec63e50dc075e7");

    var service = scope.ServiceProvider.GetRequiredService<UniswapV3PoolManager>();

    var request = new UniswapPoolContext(new Web3("https://mainnet.era.zksync.io"), networkConstants);
    
    foreach (var position in await fetcher.GetAllPositionsAsync("0xeb9191d780c0aB6Ab320C5F05E41ebF81f14255f"))
    {
        if (position.Liquidity == 0)
        {
            continue;
        }
        
        var tokens = await service.GetClaimableFeeAsync(request, position);
        var positionDetails = await service.GetPositionDetailsAsync(request, position);

        var tokensEnriched = await poolFactory.EnrichAsync(request.Web3, tokens);
        var positionDetailsEnriched = await poolFactory.EnrichAsync(request.Web3, positionDetails);
    }
}
 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.Run();