using Ardalis.Specification;
using CryptoWatcher.Modules.Uniswap.Entities;

namespace CryptoWatcher.Modules.Uniswap.Specifications;

public sealed class LiquidityPositionByIds : Specification<UniswapLiquidityPosition>
{
    public LiquidityPositionByIds(ulong positionId)
    {
        Query.Where(position => position.PositionId == positionId);
    }

    public LiquidityPositionByIds(IEnumerable<ulong> positionIds)
    {
        Query.Where(position => positionIds.Contains(position.PositionId));
    }
}