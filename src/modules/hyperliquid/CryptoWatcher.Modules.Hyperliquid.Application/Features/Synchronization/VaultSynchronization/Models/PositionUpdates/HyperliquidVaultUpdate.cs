using CryptoWatcher.Abstractions.CacheFlows;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization.Models.PositionUpdates;

public class HyperliquidVaultUpdate
{
    public required decimal Amount { get; init; }

    public required DateTimeOffset Timestamp { get; init; }
    
    public required EvmAddress VaultAddress { get; set; } = null!;

    public required TransactionHash Hash { get; set; } = null!;

    public CashFlowEvent GetCashFlowEvent()
    {
        return this is HyperliquidDepositUpdate ? CashFlowEvent.Deposit : CashFlowEvent.Withdrawal;
    }
}