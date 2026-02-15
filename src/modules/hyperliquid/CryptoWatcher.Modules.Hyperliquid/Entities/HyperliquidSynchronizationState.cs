using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Hyperliquid.Entities;

public class HyperliquidSynchronizationState
{
    public HyperliquidSynchronizationState(EvmAddress walletAddress)
    {
        WalletAddress = walletAddress;
    }

    public EvmAddress WalletAddress { get; private set; }

    public DateTimeOffset? LastProcessedEventTimestamp  { get; private set; }

    public TransactionHash? LastTransactionHash { get; private set; }
    
    public void UpdateLastProcessedEvent(DateTimeOffset eventTimestamp, TransactionHash hash)
    {
        LastProcessedEventTimestamp = eventTimestamp;
        LastTransactionHash = hash;
    }
}