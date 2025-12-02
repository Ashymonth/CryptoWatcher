using AutoFixture;
using Bogus;
using CryptoWatcher.Abstractions.CacheFlows;
using CryptoWatcher.Exceptions;
using CryptoWatcher.Extensions;
using CryptoWatcher.Modules.Uniswap.Entities;
using CryptoWatcher.Modules.Uniswap.Tests.Customizations;
using CryptoWatcher.Modules.Uniswap.Tests.DataSets;
using CryptoWatcher.Modules.Uniswap.Tests.Fakers;
using CryptoWatcher.Shared.ValueObjects;
using CryptoWatcher.ValueObjects;
using JetBrains.Annotations;
using Shouldly;

namespace CryptoWatcher.Modules.Uniswap.Tests.Entities;

[TestSubject(typeof(UniswapLiquidityPosition))]
public class UniswapLiquidityPositionTest
{
    private readonly Fixture _fixture;

    public UniswapLiquidityPositionTest()
    {
        _fixture = new Fixture();
        _fixture.Customize(new EvmAddressCustomization());
    }

    [Fact]
    public void UniswapLiquidityPositionTest_ShouldInitializeWithCorrectFields()
    {
        var chain = new UniswapChainConfigurationFaker().Generate();
        var positionFaker = new UniswapLiquidityPositionFaker(chain).Generate();

        var actual = new UniswapLiquidityPosition(positionFaker.PositionId, positionFaker.TickLower,
            positionFaker.TickUpper, positionFaker.Token0,
            positionFaker.Token1, positionFaker.WalletAddress, chain);

        actual.PositionId.ShouldBe(positionFaker.PositionId);
        actual.TickLower.ShouldBe(positionFaker.TickLower);
        actual.TickUpper.ShouldBe(positionFaker.TickUpper);
        actual.Token0.ShouldBeEquivalentTo(positionFaker.Token0);
        actual.Token1.ShouldBeEquivalentTo(positionFaker.Token1);
        actual.IsActive.ShouldBeTrue();
        actual.WalletAddress.ShouldBe(positionFaker.WalletAddress);
        actual.NetworkName.ShouldBe(chain.Name);
        actual.ProtocolVersion.ShouldBe(chain.ProtocolVersion);
        actual.PoolPositionSnapshots.ShouldBeEmpty();
        actual.CashFlows.ShouldBeEmpty();
    }

    [Fact]
    public void UniswapLiquidityPositionTest_SameTokens_ShouldThrowException()
    {
        Should.Throw<DomainException>(static () =>
        {
            var chain = new UniswapChainConfigurationFaker().Generate();
            var position = new UniswapLiquidityPositionFaker(chain).Generate();

            _ = new UniswapLiquidityPosition(position.PositionId, position.TickLower,
                position.TickUpper, position.Token0,
                position.Token0, position.WalletAddress, chain);
        }, "For uniswap position tokens can't be the same");
    }

    [Fact]
    public void UniswapLiquidityPositionTest_TickLowerGreatestThatTickUpper_ShouldThrowException()
    {
        Should.Throw<DomainException>(static () =>
        {
            var chain = new UniswapChainConfigurationFaker().Generate();
            var position = new UniswapLiquidityPositionFaker(chain).Generate();

            _ = new UniswapLiquidityPosition(position.PositionId, position.TickUpper,
                position.TickLower, position.Token0,
                position.Token0, position.WalletAddress, chain);
        }, "For uniswap position tokens can't be the same");
    }

    [Fact]
    public void UniswapLiquidityPositionAddOrUpdateSnapshotTest_WhenSnapshotNotExists_ShouldAddSnapshot()
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
    public void UniswapLiquidityPositionAddOrUpdateSnapshotTest_WhenSnapshotExists_ShouldUpdateSnapshot()
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

    [Fact]
    public void CalculateFeeInUsdTest_NoFeeClaims_ShouldCalculateLastFee()
    {
        // Arrange
        var faker = new Faker();
        var token0 = faker.Crypto().TokenInfo();
        var token1 = faker.Crypto().TokenInfoOtherThan(token0);

        var chain = new UniswapChainConfigurationFaker().Generate();
        var position = new UniswapLiquidityPositionFaker(chain).Generate();

        var startDate = DateOnly.FromDateTime(DateTime.Now);
        var endDate = startDate.AddDays(1);

        var snapshot = position.AddOrUpdateSnapshot(startDate, true, faker.Crypto().RandomTokenInfoWithFee(token0),
            faker.Crypto().RandomTokenInfoWithFee(token1));

        var snapshot1 = position.AddOrUpdateSnapshot(startDate, true, faker.Crypto().RandomTokenInfoWithFee(token0),
            faker.Crypto().RandomTokenInfoWithFee(token1));

        // Act
        var actual = position.CalculateFeeInUsd(startDate, endDate);

        // Assert
        Assert.Equal(snapshot1.FeeInUsd, actual.Value);
    }

    [Fact]
    public void CalculateFeeInUsdTest_WithFeeClaims_ShouldCalculateLastFeeWithClaimedFee()
    {
        // Arrange
        var faker = new Faker();
        var token0 = faker.Crypto().TokenInfo();
        var token1 = faker.Crypto().TokenInfoOtherThan(token0);


        var position = new UniswapLiquidityPosition(
            _fixture.Create<ulong>(),
            _fixture.Create<long>(),
            _fixture.Create<long>(),
            token0,
            token1,
            _fixture.Create<EvmAddress>(),
            _fixture.Create<UniswapChainConfiguration>());

        var startDate = DateOnly.FromDateTime(DateTime.Now);
        var endDate = startDate.AddDays(1);

        var snapshot = CreateSnapshot(startDate, token0, token1, 50);

        var snapshot1 = CreateSnapshot(endDate, token0, token1, 100);

        var tokenPair = new TokenInfoPair
        {
            Token0 = new TokenInfoWithAddress(token0, faker.Crypto().EvmAddress()),
            Token1 = new TokenInfoWithAddress(token1, faker.Crypto().EvmAddress())
        };

        var network = _fixture.Create<UniswapChainConfiguration>();
        var feeClaim = UniswapLiquidityPositionCashFlow.CreateFromEvent(
            CacheFlowEvent.FeeClaim,
            faker.Random.ULong(),
            network,
            faker.Crypto().TxHash(),
            tokenPair,
            snapshot1.Day.ToMaxDateTime());

        // position.CashFlows.Add(feeClaim);
        //
        // position.PoolPositionSnapshots.Add(snapshot);
        // position.PoolPositionSnapshots.Add(snapshot1);

        // Act
        var actual = position.CalculateFeeInUsd(startDate, endDate);

        // Assert
        Assert.Equal(snapshot1.FeeInUsd + feeClaim.Token0.FeeAmountInUsd + feeClaim.Token1.FeeAmountInUsd,
            actual.Value);
    }

    private UniswapLiquidityPositionSnapshot CreateSnapshot(DateOnly startDate, TokenInfo token0, TokenInfo token1,
        decimal feeInUsd)
    {
        return _fixture.Build<UniswapLiquidityPositionSnapshot>()
            .With(s => s.Day, startDate)
            .With(s => s.Token0, () => TokenInfoWithFee.Create(token0, feeInUsd, 50))
            .With(s => s.Token1, () => TokenInfoWithFee.Create(token1, feeInUsd, 50))
            .Create();
    }
}