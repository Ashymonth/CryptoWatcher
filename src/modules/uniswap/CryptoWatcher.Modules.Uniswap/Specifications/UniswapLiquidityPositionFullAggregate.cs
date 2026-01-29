using Ardalis.Specification;
using CryptoWatcher.Modules.Uniswap.Entities;

namespace CryptoWatcher.Modules.Uniswap.Specifications;

public class UniswapLiquidityPositionFullAggregate : Specification<UniswapLiquidityPosition>
{
    public UniswapLiquidityPositionFullAggregate(bool includeClosed = false)
    {
        Query
            .Where(position => position.IsClosed == includeClosed)
            .Include(position => position.Snapshots)
            .Include(position => position.CashFlows);
    }

    public UniswapLiquidityPositionFullAggregate(UniswapChainConfiguration chain, IEnumerable<ulong> positionIds)
    {
        Query.Where(position => positionIds.Contains(position.PositionId) &&
                                position.ProtocolVersion == chain.ProtocolVersion &&
                                position.NetworkName == chain.Name)
            .Include(position => position.Snapshots)
            .Include(position => position.CashFlows);
    }
}