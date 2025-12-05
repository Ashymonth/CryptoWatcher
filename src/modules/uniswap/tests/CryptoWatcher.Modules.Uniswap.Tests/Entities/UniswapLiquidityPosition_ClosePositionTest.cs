using CryptoWatcher.Exceptions;
using CryptoWatcher.Modules.Uniswap.Tests.Fakers;
using Shouldly;

namespace CryptoWatcher.Modules.Uniswap.Tests.Entities;

public partial class UniswapLiquidityPositionTest
{
    [Fact]
    public void UniswapLiquidityPosition_ClosePositionTest_PositionIsNotClosed_ShouldClosePosition()
    {
        var chain = new UniswapChainConfigurationFaker().Generate();
        var position = new UniswapLiquidityPositionFaker(chain).Generate();

        var closeDate = _faker.Date.FutureDateOnly();
        position.ClosePosition(closeDate);
        
        position.ClosedAt.ShouldBe(closeDate);
    }
    
    [Fact]
    public void UniswapLiquidityPosition_ClosePositionTest_PositionIsClosed_ShouldThrowException()
    {
        var chain = new UniswapChainConfigurationFaker().Generate();
        var position = new UniswapLiquidityPositionFaker(chain).Generate();

        var closeDate = _faker.Date.FutureDateOnly();
        position.ClosePosition(closeDate);

        Should.Throw<DomainException>(() => position.ClosePosition(closeDate),
            "Can't close already closed uniswap position");
    }
}