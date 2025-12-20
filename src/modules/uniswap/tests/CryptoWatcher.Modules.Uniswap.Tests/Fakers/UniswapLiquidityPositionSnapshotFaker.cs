using Bogus;
using CryptoWatcher.Modules.Uniswap.Entities;
using CryptoWatcher.Modules.Uniswap.Tests.DataSets;

namespace CryptoWatcher.Modules.Uniswap.Tests.Fakers;

public sealed class UniswapLiquidityPositionSnapshotFaker : Faker<UniswapLiquidityPositionSnapshot>
{
    public UniswapLiquidityPositionSnapshotFaker(UniswapLiquidityPosition position, DateOnly day)
    {
        CustomInstantiator(faker => new UniswapLiquidityPositionSnapshot(
            position,
            day,
            faker.Random.Bool(),
            faker.Crypto().RandomCryptoTokenStatisticWithFee(),
            faker.Crypto().RandomCryptoTokenStatisticWithFee()));
    }
}