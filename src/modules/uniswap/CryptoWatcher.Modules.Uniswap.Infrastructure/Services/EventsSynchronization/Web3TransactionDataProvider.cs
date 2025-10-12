using CryptoWatcher.Modules.Uniswap.Application.Abstractions;
using CryptoWatcher.Modules.Uniswap.Application.Models;
using CryptoWatcher.Modules.Uniswap.Entities;
using CryptoWatcher.ValueObjects;
using Nethereum.Hex.HexConvertors.Extensions;

namespace CryptoWatcher.Modules.Uniswap.Infrastructure.Services.EventsSynchronization;

internal class Web3TransactionDataProvider : ITransactionDataProvider
{
    private readonly IWeb3Factory _web3Factory;
    private readonly ILiquidityEventLogEnricher _liquidityEventLogEnricher;

    public Web3TransactionDataProvider(IWeb3Factory web3Factory, ILiquidityEventLogEnricher liquidityEventLogEnricher)
    {
        _web3Factory = web3Factory;
        _liquidityEventLogEnricher = liquidityEventLogEnricher;
    }

    public async Task<TransactionData?> GetTransactionDataAsync(UniswapChainConfiguration chain,
        TransactionHash transactionHash, CancellationToken ct)
    {
        var web3 = _web3Factory.GetWeb3(chain);
        var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);

        var wallets = chain.LiquidityPoolPositions.Select(position => position.WalletAddress)
            .Distinct()
            .ToDictionary(wallet => wallet);

        var fromAddress = EvmAddress.Create(receipt.From);
        if (!wallets.TryGetValue(fromAddress, out var walletAddress))
        {
            return null;
        }

        var liquidityEventLogs = receipt.Logs.Select(log => new LiquidityEventLog
        {
            Address = log.Address,
            Data = log.Data.HexToBigInteger(false),
            Topics = log.Topics.Select(o => o.ToString()).ToArray()!
        }).ToArray();
        
        var eventEnrichment =
            await _liquidityEventLogEnricher.EnrichLiquidityEventFromLogsAsync(walletAddress, transactionHash,
                liquidityEventLogs, ct);

        if (eventEnrichment is null)
        {
            return null;
        }

        return new TransactionData
        {
            WalletAddress = fromAddress,
            EventEnrichment = eventEnrichment,
            TransactionHash = receipt.TransactionHash
        };
    }
}