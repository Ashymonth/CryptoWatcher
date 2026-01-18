using CryptoWatcher.Modules.Uniswap.Application.Abstractions;
using CryptoWatcher.Modules.Uniswap.Application.Models;
using CryptoWatcher.Modules.Uniswap.Models;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Uniswap.Infrastructure.Services.EventsSynchronization.V3;

public class UniswapV3MintEventDecoder : ILiquidityPoolEventDecoder
{
    public BlockchainLogType LogType => BlockchainLogType.Mint;
    
    public LiquidityPoolPositionEvent DecodeModifyLiquidityEvent(EvmAddress walletAddress, BlockchainLogEntry blockchainLogEntry,
        TokenPair tokenPair, DateTime timestamp)
    {
        throw new NotImplementedException();
    }
}