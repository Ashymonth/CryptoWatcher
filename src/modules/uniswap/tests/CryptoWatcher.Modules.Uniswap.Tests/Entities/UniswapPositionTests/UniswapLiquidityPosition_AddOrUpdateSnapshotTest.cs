using Bogus;
using CryptoWatcher.Exceptions;
using CryptoWatcher.Modules.Uniswap.Entities;
using CryptoWatcher.Modules.Uniswap.Tests.DataSets;
using CryptoWatcher.Modules.Uniswap.Tests.Fakers;
using Shouldly;

namespace CryptoWatcher.Modules.Uniswap.Tests.Entities.UniswapPositionTests;

public partial class UniswapLiquidityPositionTest
{
    [Fact]
    public void Add_snapshot_for_position_if_not_exists_for_provided_date()
    {
        var chain = new UniswapChainConfigurationFaker().Generate();
        var position = new UniswapLiquidityPositionFaker(chain).Generate();

        var faker = new Faker();
        var expectedDate = DateOnly.FromDateTime(faker.Date.Future());
        const bool expectedIsInRange = true;
        var expectedToken0 = faker.Crypto().RandomCryptoTokenStatisticWithFee();
        var expectedToken1 = faker.Crypto().RandomCryptoTokenStatisticWithFee();

        var actual = position.AddOrUpdateSnapshot(expectedDate, expectedIsInRange, expectedToken0, expectedToken1);

        actual.PoolPositionId.ShouldBe(position.PositionId);
        actual.NetworkName.ShouldBe(chain.Name);
        actual.Day.ShouldBe(expectedDate);
        actual.IsInRange.ShouldBe(expectedIsInRange);
        actual.Token0.ShouldBeEquivalentTo(expectedToken0);
        actual.Token1.ShouldBeEquivalentTo(expectedToken1);
    }

    [Fact]
    public void Update_snapshot_for_position_if_exists()
    {
        var chain = new UniswapChainConfigurationFaker().Generate();
        var position = new UniswapLiquidityPositionFaker(chain).Generate();

        var faker = new Faker();
        var expectedDate = DateOnly.FromDateTime(faker.Date.Future());
        const bool expectedIsInRange = true;

        var firstToken0 = faker.Crypto().RandomCryptoTokenStatisticWithFee();
        var secondToken1 = faker.Crypto().RandomCryptoTokenStatisticWithFee();

        var createdSnapshot = position.AddOrUpdateSnapshot(expectedDate, expectedIsInRange, firstToken0, secondToken1);

        var updatedToken0 = faker.Crypto().RandomCryptoTokenStatisticWithFee();
        var updatedToken1 = faker.Crypto().RandomCryptoTokenStatisticWithFee();

        var updatedSnapshot =
            position.AddOrUpdateSnapshot(expectedDate, !expectedIsInRange, updatedToken0, updatedToken1);

        updatedSnapshot.PoolPositionId.ShouldBe(position.PositionId);
        updatedSnapshot.NetworkName.ShouldBe(chain.Name);
        updatedSnapshot.Day.ShouldBe(expectedDate);
        updatedSnapshot.IsInRange.ShouldBe(!expectedIsInRange);
        updatedSnapshot.Token0.ShouldBeEquivalentTo(createdSnapshot.Token0);
        updatedSnapshot.Token1.ShouldBeEquivalentTo(createdSnapshot.Token1);
    }

    [Fact]
    public void Do_not_add_snapshot_if_position_closed()
    {
        var position = CreatePositionWithSnapshots(_faker.Date.FutureDateOnly(), 10);
        position.ClosePosition(_faker.Date.FutureDateOnly());

        var token0 = _faker.Crypto().RandomCryptoTokenStatisticWithFee();
        var token1 = _faker.Crypto().RandomCryptoTokenStatisticWithFee();

        Should.Throw<DomainException>(
            () => { position.AddOrUpdateSnapshot(_faker.Date.FutureDateOnly(), false, token0, token1); },
            UniswapLiquidityPosition.PositionClosedException);
    }
}