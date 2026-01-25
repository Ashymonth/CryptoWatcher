using System.Numerics;
using CryptoWatcher.Modules.Uniswap.Entities;
using CryptoWatcher.Modules.Uniswap.Infrastructure.Services;
using CryptoWatcher.ValueObjects;
using Nethereum.RPC.Eth.DTOs;

namespace CryptoWatcher.Modules.Uniswap.Infrastructure.Integrations.Blockchain.Api;

public interface IWeb3BlockchainApi
{
    Task<TransactionReceipt> GetTransactionReceiptAsync(UniswapChainConfiguration chain,
        TransactionHash transactionHash);

    Task<DateTime> GetTransactionTimestampAsync(UniswapChainConfiguration chain, BigInteger blockNumber);
}

public class Web3BlockchainApi : IWeb3BlockchainApi
{
    private readonly IWeb3Factory _web3Factory;

    public Web3BlockchainApi(IWeb3Factory web3Factory)
    {
        _web3Factory = web3Factory;
    }

    public async Task<TransactionReceipt> GetTransactionReceiptAsync(UniswapChainConfiguration chain,
        TransactionHash transactionHash)
    {
        var web3 = _web3Factory.GetWeb3(chain);

        return await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
    }

    public async Task<DateTime> GetTransactionTimestampAsync(UniswapChainConfiguration chain, BigInteger blockNumber)
    {
        var web3 = _web3Factory.GetWeb3(chain);

        var result = await web3.Eth.Blocks.GetBlockTransactionCountByNumber.SendRequestAsync(blockNumber);

        return DateTimeOffset.FromUnixTimeMilliseconds((long)result.Value).UtcDateTime;
    }
}