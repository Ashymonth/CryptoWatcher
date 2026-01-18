using CryptoWatcher.Modules.Uniswap.Infrastructure.UniswapV3.Abstractions;
using Nethereum.RPC.Eth.DTOs;

namespace CryptoWatcher.Modules.Uniswap.Infrastructure.Services;

internal interface IUniswapTransactionLogsDecoderFactory
{
    ITransactionLogEventDecoder? FindDecoder(TransactionReceipt transactionReceipt);
}