using CryptoWatcher.Modules.Uniswap.Application.Services.PoisitionEventsSync.UniswapV3.Models.Operations;
using Nethereum.RPC.Eth.DTOs;

namespace CryptoWatcher.Modules.Uniswap.Infrastructure.UniswapV3.Abstractions;

public interface ITransactionLogEventDecoder
{
    bool CanDecode(TransactionReceipt transactionReceipt);
    
    PositionOperation GetOperation(TransactionReceipt transactionReceipt);
}