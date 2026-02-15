using CryptoWatcher.Modules.Hyperliquid.Entities;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Hyperliquid.Application.Features.Reports.Abstractions;

public interface IHyperliquidPositionForReportQuery
{
    Task<HyperliquidVaultPosition[]> GetPositionsAsync(IReadOnlyCollection<EvmAddress> wallets,
        DateOnly from, DateOnly to,
        CancellationToken ct = default);
}