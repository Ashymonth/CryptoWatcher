using CryptoWatcher.Modules.Uniswap.Application.Models;
using CryptoWatcher.Modules.Uniswap.Models;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Uniswap.Infrastructure.Services;

public interface IUniswapLiquidityPoolEventDecoderSelector
{
    LiquidityPoolPositionEvent DecodeEvent(EvmAddress walletAddress,
        BlockchainLogEntry blockchainLogEntry,
        TokenPair tokenPair, DateTime timestamp);
}