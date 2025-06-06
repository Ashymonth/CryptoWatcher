using System.Numerics;
using CryptoWatcher.Entities;
using Nethereum.Web3;
using UniswapClient.Models;

namespace CryptoWatcher.Application.Uniswap;

public abstract class UniswapPositionFetcherBase
{
    public async Task<List<IUniswapPosition>> GetPositionsAsync(Network network,
        Wallet wallet)
    {
        var web3 = new Web3(network.RpcUrl);
        var balance = await GetBalanceAsync(web3, network, wallet);

        return await GetPositionsDataAsync(web3, network, wallet, balance);
    }

    protected abstract Task<List<IUniswapPosition>> GetPositionsDataAsync(IWeb3 web3,
        Network network,
        Wallet wallet,
        BigInteger balance);

    private static async Task<BigInteger> GetBalanceAsync(Web3 web3, Network network, Wallet wallet)
    {
        return await web3.Eth.ERC20.GetContractService(network.NftManagerAddress).BalanceOfQueryAsync(wallet.Address);
    }
}