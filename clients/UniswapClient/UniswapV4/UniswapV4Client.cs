using UniswapClient.UniswapV4.LiquidityPool;
using UniswapClient.UniswapV4.PositionsFetcher;
using UniswapClient.UniswapV4.StateView;

namespace UniswapClient.UniswapV4;

public class UniswapV4Client
{
    public UniswapV4Client(IUniswapV4LiquidityPool liquidityPool, IUniswapV4StateView stateView, IUniswapV4PositionFetcher positionFetcher)
    {
        LiquidityPool = liquidityPool;
        StateView = stateView;
        PositionFetcher = positionFetcher;
    }

    public IUniswapV4LiquidityPool LiquidityPool { get; }
    
    public IUniswapV4StateView StateView { get; }
    
    public IUniswapV4PositionFetcher PositionFetcher { get; }
}