using CryptoWatcher.Modules.Uniswap.Entities;

namespace CryptoWatcher.Modules.Uniswap.Application.Models;

public record WalletEventExtractionResult
{
    public required UniswapLiquidityPosition[] UpdatedPositions { get; init; }

    public required BlockchainTransaction LastScannedTransaction { get; init; }
}