using CryptoWatcher.Services;

namespace CryptoWatcher.Models;

public record PositionWithPool(UniswapV3PositionFetcher.PositionData Position, PoolFactoryService.PoolInfo PoolAddress);