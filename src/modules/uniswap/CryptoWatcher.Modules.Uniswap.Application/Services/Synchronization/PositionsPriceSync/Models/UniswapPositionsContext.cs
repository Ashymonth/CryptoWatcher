using CryptoWatcher.Modules.Uniswap.Entities;
using CryptoWatcher.Shared.Entities;

namespace CryptoWatcher.Modules.Uniswap.Application.Services.Synchronization.PositionsPriceSync.Models;

public sealed class UniswapPositionsContext
{
    private IReadOnlyList<UniswapLiquidityPosition> Positions { get; }

    public UniswapPositionsContext(IReadOnlyList<UniswapLiquidityPosition> positions)
    {
        Positions = positions;
    }

    public UniswapLiquidityPosition[] ForChainAndWallet(UniswapChainConfiguration chain, Wallet wallet)
    {
        return Positions.Where(position =>
                position.WalletAddress.Equals(wallet.Address) &&
                position.NetworkName == chain.Name &&
                position.ProtocolVersion == chain.ProtocolVersion)
            .ToArray();
    }
}