using CryptoWatcher.Shared.Entities;

namespace CryptoWatcher.HyperliquidModule.Abstractions;

public interface IHyperliquidReportService
{
    Task<Stream> GenerateReportAsync(Wallet wallet, CancellationToken ct = default);
}