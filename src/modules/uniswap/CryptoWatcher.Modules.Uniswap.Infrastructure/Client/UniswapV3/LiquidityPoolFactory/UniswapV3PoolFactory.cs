using CryptoWatcher.Modules.Uniswap.Infrastructure.Client.UniswapV3.LiquidityPoolFactory.Contracts;
using Nethereum.Web3;

namespace CryptoWatcher.Modules.Uniswap.Infrastructure.Client.UniswapV3.LiquidityPoolFactory;

internal interface IUniswapV3PoolFactory
{
    Task<string> GetPoolAddressAsync(IWeb3 web3, string poolFactoryAddress, string token0, string token1, uint fee);
}

internal class UniswapV3PoolFactory : IUniswapV3PoolFactory
{
    public async Task<string> GetPoolAddressAsync(IWeb3 web3, string poolFactoryAddress, string token0, string token1,
        uint fee)
    {
        var contract = web3.Eth.GetContract(PoolFactoryAbi.Abi, poolFactoryAddress);
        var function = contract.GetFunction("getPool");

        var result = await function.CallAsync<string>(token0, token1, fee);
        return result;
    }
}