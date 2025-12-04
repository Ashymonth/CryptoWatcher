using Bogus;
using CryptoWatcher.Modules.Uniswap.Tests.DataSets;
using CryptoWatcher.Modules.Uniswap.Tests.Fakers;
using Shouldly;

namespace CryptoWatcher.Modules.Uniswap.Tests.Entities;

public partial class UniswapLiquidityPositionTest
{
    [Fact]
    public void UniswapLiquidityPosition_AddOrUpdateSnapshotTest_WhenSnapshotNotExists_ShouldAddSnapshot()
    {
        var chain = new UniswapChainConfigurationFaker().Generate();
        var position = new UniswapLiquidityPositionFaker(chain).Generate();

        var faker = new Faker();
        var expectedDate = DateOnly.FromDateTime(faker.Date.Future());
        const bool expectedIsInRange = true;
        var expectedToken0 = faker.Crypto().RandomTokenInfoWithFee(position.Token0);
        var expectedToken1 = faker.Crypto().RandomTokenInfoWithFee(position.Token1);

        var actual = position.AddOrUpdateSnapshot(expectedDate, expectedIsInRange, expectedToken0, expectedToken1);

        actual.PoolPositionId.ShouldBe(position.PositionId);
        actual.NetworkName.ShouldBe(chain.Name);
        actual.Day.ShouldBe(expectedDate);
        actual.IsInRange.ShouldBe(expectedIsInRange);
        actual.Token0.ShouldBeEquivalentTo(expectedToken0);
        actual.Token1.ShouldBeEquivalentTo(expectedToken1);
    }

    [Fact]
    public void UniswapLiquidityPosition_AddOrUpdateSnapshotTest_WhenSnapshotExists_ShouldUpdateSnapshot()
    {
        var chain = new UniswapChainConfigurationFaker().Generate();
        var position = new UniswapLiquidityPositionFaker(chain).Generate();

        var faker = new Faker();
        var expectedDate = DateOnly.FromDateTime(faker.Date.Future());
        const bool expectedIsInRange = true;

        var firstToken0 = faker.Crypto().RandomTokenInfoWithFee(position.Token0);
        var secondToken1 = faker.Crypto().RandomTokenInfoWithFee(position.Token1);

        var createdSnapshot = position.AddOrUpdateSnapshot(expectedDate, expectedIsInRange, firstToken0, secondToken1);

        var updatedToken0 = faker.Crypto().RandomTokenInfoWithFee(position.Token0);
        var updatedToken1 = faker.Crypto().RandomTokenInfoWithFee(position.Token1);

        var updatedSnapshot =
            position.AddOrUpdateSnapshot(expectedDate, !expectedIsInRange, updatedToken0, updatedToken1);

        updatedSnapshot.PoolPositionId.ShouldBe(position.PositionId);
        updatedSnapshot.NetworkName.ShouldBe(chain.Name);
        updatedSnapshot.Day.ShouldBe(expectedDate);
        updatedSnapshot.IsInRange.ShouldBe(!expectedIsInRange);
        createdSnapshot.Token0.ShouldBeEquivalentTo(updatedToken0);
        createdSnapshot.Token1.ShouldBeEquivalentTo(updatedToken1);
    }
}