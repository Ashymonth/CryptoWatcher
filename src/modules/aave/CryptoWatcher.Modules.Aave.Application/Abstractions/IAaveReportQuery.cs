using CryptoWatcher.Modules.Aave.Entities;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Aave.Application.Abstractions;

public interface IAaveReportQuery
{
    Task<IReadOnlyList<AavePosition>> GetPositionsForReportAsync(
        IReadOnlyCollection<EvmAddress> wallets,
        DateOnly from,
        DateOnly to,
        CancellationToken ct);
}