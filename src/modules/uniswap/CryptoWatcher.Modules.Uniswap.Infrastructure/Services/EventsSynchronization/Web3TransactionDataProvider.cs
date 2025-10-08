using CryptoWatcher.Modules.Uniswap.Application.Abstractions;
using CryptoWatcher.Modules.Uniswap.Application.Models;
using CryptoWatcher.Modules.Uniswap.Entities;

namespace CryptoWatcher.Modules.Uniswap.Infrastructure.Services.EventsSynchronization;

internal class Web3TransactionDataProvider : ITransactionDataProvider
{
    private readonly IWeb3Factory _web3Factory;
    private readonly IUnichainLogReader _unichainLogReader;

    public Web3TransactionDataProvider(IWeb3Factory web3Factory, IUnichainLogReader unichainLogReader)
    {
        _web3Factory = web3Factory;
        _unichainLogReader = unichainLogReader;
    }

    public async Task<TransactionData> GetTransactionDataAsync(UniswapChainConfiguration chainConfiguration,
        string transactionHash, CancellationToken ct)
    {
        var web3 = _web3Factory.GetWeb3(chainConfiguration);
        var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
        
        var tokenPair = await _unichainLogReader.ReadTokenPairFromLogAsync(
            transactionHash, receipt.Logs, ct);

        return new TransactionData
        {
            WalletAddress = receipt.From,
            TokenPair = tokenPair
        };
    }
}