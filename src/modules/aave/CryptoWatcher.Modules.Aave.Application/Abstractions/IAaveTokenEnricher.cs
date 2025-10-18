using CryptoWatcher.Modules.Aave.Application.Models;
using CryptoWatcher.Modules.Aave.Entities;
using CryptoWatcher.Modules.Aave.Models;
using CryptoWatcher.Shared.ValueObjects;

namespace CryptoWatcher.Modules.Aave.Application.Abstractions;

public interface IAaveTokenEnricher
{
    Task<TokenInfo> EnrichTokenAsync(AaveChainConfiguration chain, CalculatableAaveLendingPosition position,
        CancellationToken ct = default);
}