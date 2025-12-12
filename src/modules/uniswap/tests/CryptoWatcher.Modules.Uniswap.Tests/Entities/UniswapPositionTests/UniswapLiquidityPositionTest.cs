using CryptoWatcher.Exceptions;
using CryptoWatcher.Modules.Uniswap.Entities;
using CryptoWatcher.Modules.Uniswap.Tests.Fakers;
using JetBrains.Annotations;
using Shouldly;

namespace CryptoWatcher.Modules.Uniswap.Tests.Entities.UniswapPositionTests;

[TestSubject(typeof(UniswapLiquidityPosition))]
public partial class UniswapLiquidityPositionTest
{
    [Fact]
    public void Position_initialized_with_correct_fields()
    {
        var createdAt = _faker.Date.FutureDateOnly();
        
        var chain = new UniswapChainConfigurationFaker().Generate();
        var position = new UniswapLiquidityPositionFaker(chain)
            .RuleFor(liquidityPosition => liquidityPosition.CreatedAt, createdAt)
            .Generate();
        
        var actual = new UniswapLiquidityPosition(position.PositionId, position.TickLower,
            position.TickUpper, position.Token0,
            position.Token1, position.WalletAddress, chain, createdAt);

        actual.ShouldBeEquivalentTo(position);
    }

    [Fact]
    public void Creating_position_with_identical_tokens_throws_exception()
    {
        Should.Throw<DomainException>(() =>
        {
            var chain = new UniswapChainConfigurationFaker().Generate();
            var position = new UniswapLiquidityPositionFaker(chain).Generate();
            var createdAt = _faker.Date.FutureDateOnly();

            _ = new UniswapLiquidityPosition(position.PositionId, position.TickLower,
                position.TickUpper, position.Token0,
                position.Token0, position.WalletAddress, chain, createdAt);
        }, UniswapLiquidityPosition.SameSymbolsException);
    }

    [Fact]
    public void Creating_position_with_invalid_tick_range_throws_exception()
    {
        Should.Throw<DomainException>(() =>
        {
            var chain = new UniswapChainConfigurationFaker().Generate();
            var position = new UniswapLiquidityPositionFaker(chain).Generate();

            var createdAt = _faker.Date.FutureDateOnly();

            _ = new UniswapLiquidityPosition(position.PositionId, position.TickUpper,
                position.TickLower, position.Token0,
                position.Token0, position.WalletAddress, chain, createdAt);
        }, UniswapLiquidityPosition.InvalidTickRangeException);
    }
}