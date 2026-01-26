using CryptoWatcher.Modules.Uniswap.Application.Services.PoisitionEventsSync.UniswapV3.Models.Operations;

namespace CryptoWatcher.Modules.Uniswap.Application.Models;

public class WalletTransactionScanResult
{
    public UniswapEvent? Event { get; set; }

    public BlockchainTransaction Transaction { get; set; } = null!;
}