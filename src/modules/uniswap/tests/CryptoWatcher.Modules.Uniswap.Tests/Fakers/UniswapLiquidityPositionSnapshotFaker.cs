using Bogus;
using CryptoWatcher.Modules.Uniswap.Entities;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Uniswap.Tests.Fakers;

public sealed class UniswapLiquidityPositionSnapshotFaker : Faker<UniswapLiquidityPositionSnapshot>
{
    public UniswapLiquidityPositionSnapshotFaker(UniswapLiquidityPosition position, DateOnly day)
    {
        CustomInstantiator(faker => new UniswapLiquidityPositionSnapshot(
                position,
                day,
                faker.Random.Bool(),
                TokenInfoWithFee.Create(position.Token0, faker.Random.Decimal(), faker.Random.Decimal()),
                TokenInfoWithFee.Create(position.Token1, faker.Random.Decimal(), faker.Random.Decimal())));
    }
}