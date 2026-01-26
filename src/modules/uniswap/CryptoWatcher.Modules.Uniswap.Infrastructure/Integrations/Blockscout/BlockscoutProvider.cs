using System.Numerics;
using CryptoWatcher.Modules.Uniswap.Application.Abstractions;
using CryptoWatcher.Modules.Uniswap.Application.Models;
using CryptoWatcher.Modules.Uniswap.Entities;
using CryptoWatcher.Modules.Uniswap.Infrastructure.Integrations.Blockscout.Api;
using CryptoWatcher.Modules.Uniswap.Infrastructure.Integrations.Blockscout.Contracts.TransactionHistory;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Uniswap.Infrastructure.Integrations.Blockscout;

public class BlockscoutProvider : IBlockscoutProvider
{
    private const string CallType = "call";

    private readonly IBlockscoutApi _blockscoutApi;

    public BlockscoutProvider(IBlockscoutApi blockscoutApi)
    {
        _blockscoutApi = blockscoutApi;
    }

    public async Task<BlockscoutPage> GetAccountTransactionsAsync(
        UniswapChainConfiguration chain,
        EvmAddress walletAddress,
        BlockscoutNextPageParams? nextPageParams,
        CancellationToken ct = default)
    {
        var blockscoutResponse =
            await _blockscoutApi.GetTransactionHistoryAsync(walletAddress, nextPageParams?.MapToQueryParams(), ct);

        var result = new BlockscoutPage
        {
            Transactions = blockscoutResponse.Items.Select(item => new BlockscoutTransaction
            {
                Method = item.Method,
                BlockNumber = item.BlockNumber,
                TransactionHash = item.Hash
            }).ToArray(),
            NextPageParams = blockscoutResponse.NextPageParams is not null
                ? new BlockscoutNextPageParams
                {
                    BlockNumber = blockscoutResponse.NextPageParams.BlockNumber,
                    Index = blockscoutResponse.NextPageParams.Index,
                    Hash = TransactionHash.FromString(blockscoutResponse.NextPageParams.Hash)
                }
                : null
        };

        return result;
    }

    public async Task<DateTime> GetTransactionTimestampAsync(UniswapChainConfiguration chainConfiguration,
        TransactionHash transactionHash,
        CancellationToken ct = default)
    {
        var internalTransactionsResponse = await _blockscoutApi.GetTransactionTimestampAsync(transactionHash.Value, ct);

        if (internalTransactionsResponse is null)
        {
            throw new InvalidOperationException(
                $"Can't find internal transaction with ETH. Transaction hash:{transactionHash}");
        }

        return internalTransactionsResponse.TimeStamp.UtcDateTime;
    }

    public async Task<EthTransaction> GetEthAmountFromInternalTransaction(
        UniswapChainConfiguration chainConfiguration,
        EvmAddress walletAddress,
        TransactionHash transactionHash,
        CancellationToken ct = default)
    {
        var internalTransactionsResponse = await _blockscoutApi.GetInternalTransactionsAsync(transactionHash.Value, ct);

        //there should be only one internal transaction with a call and not 0 value for wallet address
        var internalTransactionsWithEth = internalTransactionsResponse!.Items.SingleOrDefault(item =>
            item.To.Hash == walletAddress &&
            item.Value != "0" && item.Type == CallType);

        // if there is no eth then it means that pool has 100% value in second token and eth percent is empty
        if (internalTransactionsWithEth is null)
        {
            return new EthTransaction
            {
                Amount = 0,
                TimeStamp = internalTransactionsResponse.Items.First().TimeStamp,
            };
        }

        return new EthTransaction
        {
            Amount = BigInteger.Parse(internalTransactionsWithEth.Value),
            TimeStamp = internalTransactionsWithEth.TimeStamp
        };
    }
}