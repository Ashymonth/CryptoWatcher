using CryptoWatcher.Exceptions;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Hyperliquid.Entities;

public class HyperliquidVaultPeriod
{
    private HyperliquidVaultPeriod()
    {
    }

    public Guid Id { get; private init; } 
    
    public DateTimeOffset StartedAt { get; private init; }

    public DateTimeOffset? ClosedAt { get; private set; }

    public EvmAddress WalletAddress { get; private init; } = null!;

    public EvmAddress VaultAddress { get; private init; } = null!;

    public static HyperliquidVaultPeriod StartNew(DateTimeOffset startedAt, EvmAddress wallet, EvmAddress vault)
    {
        return new HyperliquidVaultPeriod
        {
            Id = Guid.CreateVersion7(),
            StartedAt = startedAt,
            WalletAddress = wallet,
            VaultAddress = vault
        };
    }

    public void Close(DateTimeOffset closedAt)
    {
        if (closedAt < StartedAt)
        {
            throw new DomainException("Close date cannot be before start date");
        }

        ClosedAt = closedAt;
    }
}