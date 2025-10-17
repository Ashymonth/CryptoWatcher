using CryptoWatcher.Modules.Aave.Models;
using CryptoWatcher.Shared.ValueObjects;

namespace CryptoWatcher.Modules.Aave.Application.Abstractions;

public interface IAaveTokenEnricher
{
    Task<TokenInfo> EnrichTokenAsync(AaveNetwork network, CalculatableAaveLendingPosition position,
        CancellationToken ct = default);
}