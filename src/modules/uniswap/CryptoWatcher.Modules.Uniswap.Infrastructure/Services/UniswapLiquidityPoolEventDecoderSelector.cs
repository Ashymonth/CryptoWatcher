using CryptoWatcher.Modules.Uniswap.Application.Abstractions;
using CryptoWatcher.Modules.Uniswap.Application.Models;
using CryptoWatcher.Modules.Uniswap.Models;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Uniswap.Infrastructure.Services;

public class UniswapLiquidityPoolEventDecoderSelector : IUniswapLiquidityPoolEventDecoderSelector
{
    private readonly Dictionary<BlockchainLogType, ILiquidityPoolEventDecoder> _decoders;

    public UniswapLiquidityPoolEventDecoderSelector(IEnumerable<ILiquidityPoolEventDecoder> decoders)
    {
        _decoders = decoders.ToDictionary(x => x.LogType);
    }
    
    public LiquidityPoolPositionEvent DecodeEvent(EvmAddress walletAddress,
        BlockchainLogEntry blockchainLogEntry,
        TokenPair tokenPair, DateTime timestamp)
    {
        if (_decoders.TryGetValue(blockchainLogEntry.Type, out var eventDecoder))
        {
            return eventDecoder.DecodeModifyLiquidityEvent(walletAddress, blockchainLogEntry, tokenPair, timestamp);
        }
        
        throw new InvalidOperationException($"Can't decode event of type {blockchainLogEntry.Type}");
    }
}