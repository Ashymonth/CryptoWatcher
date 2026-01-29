using CryptoWatcher.Modules.Uniswap.Application.Models;
using CryptoWatcher.Modules.Uniswap.Entities;

namespace CryptoWatcher.Modules.Uniswap.Application.Abstractions;

public interface IUniswapWalletSyncStore
{
    Task SaveUpdatedPositionsWithStateAsync(UniswapSynchronizationState state, WalletEventExtractionResult batch,
        CancellationToken ct = default);
}