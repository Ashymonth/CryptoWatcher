using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization.Models;

public class HyperliquidVault
{
    public EvmAddress Address { get; init; } = null!;

    public decimal Balance { get; init; }
}