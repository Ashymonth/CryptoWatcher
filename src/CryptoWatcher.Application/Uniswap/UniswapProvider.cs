using CryptoWatcher.Abstractions.Integrations;
using CryptoWatcher.Application.Uniswap.V3;
using CryptoWatcher.Application.Uniswap.V4;
using CryptoWatcher.Entities;
using CryptoWatcher.Host.Services.Uniswap.V3;
using CryptoWatcher.Models;
using Microsoft.Extensions.DependencyInjection;
using Nethereum.Web3;
using UniswapClient.Models;
using UniswapClient.UniswapV3;
using UniswapClient.UniswapV4;

namespace CryptoWatcher.Application.Uniswap;

public class UniswapProvider
{
    private readonly IServiceProvider _serviceProvider;

    public UniswapProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<List<IUniswapPosition>> GetPositionsAsync(Network network, Wallet wallet)
    {
        return network.Name switch
        {
            "ZkSync" => await _serviceProvider.GetRequiredService<UniswapV3PositionFetcher>()
                .GetPositionsAsync(network, wallet),
            "Unichain" => await _serviceProvider.GetRequiredService<UniswapV4PositionFetcher>()
                .GetPositionsAsync(network, wallet),
            _ => throw new NotImplementedException(),
        };
    }

    public async Task<LiquidityPool> GetPoolAsync(IWeb3 web3, Network network,
        IUniswapPosition position)
    {
        return network.Name switch
        {
            "ZkSync" => await _serviceProvider.GetRequiredService<IUniswapPoolProvider<UniswapV3PositionInfo>>()
                .GetPoolAsync(web3, network, (UniswapV3PositionInfo)position),

            "Unichain" => await _serviceProvider.GetRequiredService<IUniswapPoolProvider<UniswapV4PositionInfo>>()
                .GetPoolAsync(web3, network, (UniswapV4PositionInfo)position),
            _ => throw new NotImplementedException(),
        };
    }
}