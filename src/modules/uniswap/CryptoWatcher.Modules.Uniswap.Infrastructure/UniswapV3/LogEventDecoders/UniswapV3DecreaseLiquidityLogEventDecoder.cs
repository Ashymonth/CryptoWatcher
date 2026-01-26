using CryptoWatcher.Modules.Uniswap.Application.Services.PoisitionEventsSync.UniswapV3.Models.Operations;
using CryptoWatcher.Modules.Uniswap.Infrastructure.Extensions;
using CryptoWatcher.Modules.Uniswap.Infrastructure.Services;
using CryptoWatcher.Modules.Uniswap.Infrastructure.UniswapV3.Abstractions;
using CryptoWatcher.Modules.Uniswap.Infrastructure.UniswapV3.Models.Events;
using Nethereum.Contracts;
using Nethereum.Contracts.Standards.ERC20.ContractDefinition;
using Nethereum.RPC.Eth.DTOs;

namespace CryptoWatcher.Modules.Uniswap.Infrastructure.UniswapV3.LogEventDecoders;

public class UniswapV3DecreaseLiquidityLogEventDecoder : ITransactionLogEventDecoder
{
    public bool CanDecode(TransactionReceipt transactionReceipt)
    {
        return transactionReceipt.DecodeAllEvents<DecreaseLiquidityEvent>().Count == 1;
    }

    public PositionOperation GetOperation(TransactionReceipt transactionReceipt)
    {
        var decreaseEvent = transactionReceipt.DecodeAllEvents<DecreaseLiquidityEvent>().Single();

        var collectEvents = transactionReceipt.DecodeAllEvents<ManagerCollectEvent>().Single();

        var tokenTransfers = transactionReceipt.DecodeAllEvents<TransferEventDTO>();

        var (token0, token1) =
            tokenTransfers.MapEventToTokens(decreaseEvent.Event.Amount0, decreaseEvent.Event.Amount1);
        
        return new DecreaseLiquidityOperation
        {
            PositionId = (ulong)decreaseEvent.Event.TokenId,
            Token0 = token0,
            Token1 = token1,
            Commission0 = collectEvents.Event.Amount0,
            Commission1 = collectEvents.Event.Amount1,
            TransactionHash = transactionReceipt.TransactionHash,
            BlockNumber = transactionReceipt.BlockNumber,
            IsPositionClosed = decreaseEvent.Event.Amount0 == 0
        };
    }
}