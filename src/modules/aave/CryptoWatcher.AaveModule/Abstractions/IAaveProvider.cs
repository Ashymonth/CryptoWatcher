using CryptoWatcher.AaveModule.Models;
using CryptoWatcher.Shared.Entities;

namespace CryptoWatcher.AaveModule.Abstractions;

public interface IAaveProvider
{
    Task<List<AaveLendingPosition>> GetLendingPositionAsync(AaveNetwork aaveNetwork, Wallet wallet,
        CancellationToken ct = default);
}