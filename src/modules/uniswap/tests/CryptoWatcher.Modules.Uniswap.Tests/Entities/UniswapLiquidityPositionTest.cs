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
public partial class UniswapLiquidityPositionTest
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
}