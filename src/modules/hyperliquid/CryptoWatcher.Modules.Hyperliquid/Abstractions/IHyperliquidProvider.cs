using CryptoWatcher.Modules.Hyperliquid.Entities;
using CryptoWatcher.Shared.Entities;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Hyperliquid.Abstractions;

public interface IHyperliquidProvider
{
    Task<HyperliquidVaultEvent[]> GetCashFlowEventsAsync(Wallet wallet,
        DateOnly from, DateOnly to,
        CancellationToken ct = default);
    
    Task<(EvmAddress VaultAddress, decimal Equity)[]> GetVaultsPositionsEquityAsync(Wallet wallet,
        CancellationToken ct = default);
}