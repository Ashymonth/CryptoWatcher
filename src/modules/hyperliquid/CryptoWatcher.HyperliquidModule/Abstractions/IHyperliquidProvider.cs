using CryptoWatcher.HyperliquidModule.Entities;
using CryptoWatcher.Shared.Entities;

namespace CryptoWatcher.HyperliquidModule.Abstractions;

public interface IHyperliquidProvider
{
    Task<HyperliquidVaultEvent[]> GetVaultsFundingHistory(Wallet wallet,
        CancellationToken ct = default);
    
    Task<(string VaultAddress, decimal Equity)[]> GetVaultsPositionsEquityAsync(Wallet wallet,
        CancellationToken ct = default);
}