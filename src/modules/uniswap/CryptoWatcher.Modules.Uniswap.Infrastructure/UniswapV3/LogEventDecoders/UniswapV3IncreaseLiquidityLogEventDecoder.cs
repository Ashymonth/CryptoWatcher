using CryptoWatcher.Modules.Uniswap.Application.UniswapV3.Models.Operations;
using CryptoWatcher.Modules.Uniswap.Infrastructure.Extensions;
using CryptoWatcher.Modules.Uniswap.Infrastructure.Services;
using CryptoWatcher.Modules.Uniswap.Infrastructure.UniswapV3.Abstractions;
using CryptoWatcher.Modules.Uniswap.Infrastructure.UniswapV3.Models.Events;
using CryptoWatcher.ValueObjects;
using Nethereum.Contracts;
using Nethereum.Contracts.Standards.ERC20.ContractDefinition;
using Nethereum.RPC.Eth.DTOs;

namespace CryptoWatcher.Modules.Uniswap.Infrastructure.UniswapV3.LogEventDecoders;

public class UniswapV3IncreaseLiquidityLogEventDecoder : ITransactionLogEventDecoder
{
    /// <summary>
    /// Increase liquidity events must contain a single mint and a single increase in liquidity.
    /// </summary>
    /// <param name="transactionReceipt"></param>
    /// <remarks>There is a difference between a mint and a transfer ERC721 event. When it's just an increase in liquidity, there are no ERC721 transfer</remarks>
    /// <returns></returns>
    public bool CanDecode(TransactionReceipt transactionReceipt)
    {
        // increase liquidity events must contains single mint and single increase liquidity event.
        // note: there is difference between mint and transfer ERC721 event. when it's only increase liquidity 
        // then there is not any ERC721 transfer events.
        return transactionReceipt.DecodeAllEvents<MintEvent>().Count == 1 &&
               transactionReceipt.DecodeAllEvents<IncreaseLiquidityEvent>().Count == 1  &&
               transactionReceipt.DecodeAllEvents<Nethereum.Contracts.Standards.ERC721.ContractDefinition.TransferEventDTO>().Count == 0;
    }

    public PositionOperation GetOperation(TransactionReceipt transactionReceipt)
    {
        var increaseLiquidity = transactionReceipt.DecodeAllEvents<IncreaseLiquidityEvent>().Single();

        var tokenTransfers = transactionReceipt.DecodeAllEvents<TransferEventDTO>();

        var (token0, token1) =
            tokenTransfers.MapEventToTokens(increaseLiquidity.Event.Amount0, increaseLiquidity.Event.Amount1);
        
        return new IncreaseLiquidityOperation
        {
            PositionId = (ulong)increaseLiquidity.Event.TokenId,
            TransactionHash = transactionReceipt.TransactionHash,
            BlockNumber = transactionReceipt.BlockNumber,
            Token0 = token0,
            Token1 = token1
        };
    }
}