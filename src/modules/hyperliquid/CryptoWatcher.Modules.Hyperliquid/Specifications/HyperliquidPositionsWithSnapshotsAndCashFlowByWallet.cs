using Ardalis.Specification;
using CryptoWatcher.Modules.Hyperliquid.Entities;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Hyperliquid.Specifications;

public class HyperliquidPositionsWithSnapshotsAndCashFlowByWallet : Specification<HyperliquidVaultPosition>
{
    public HyperliquidPositionsWithSnapshotsAndCashFlowByWallet(EvmAddress walletAddress)
    {
        Query.Where(position => position.WalletAddress == walletAddress)
            .Include(position => position.Periods)
            .Include(position => position.Snapshots)
            .Include(position => position.CashFlows);
    }
}