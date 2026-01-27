using System.Runtime.CompilerServices;
using CryptoWatcher.Modules.Uniswap.Abstractions;
using CryptoWatcher.Modules.Uniswap.Application.Abstractions;
using CryptoWatcher.Modules.Uniswap.Entities;
using CryptoWatcher.Shared.Entities;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Uniswap.Application.Services.Synchronization.PositionsPriceSync;

public class PositionPriceSynchronizer : IPositionPriceSynchronizer
{
    private readonly IUniswapProvider _uniswapProvider;
    private readonly IPositionEvaluator _positionEvaluator;

    public PositionPriceSynchronizer(IUniswapProvider uniswapProvider, IPositionEvaluator positionEvaluator)
    {
        _uniswapProvider = uniswapProvider;
        _positionEvaluator = positionEvaluator;
    }

    public async IAsyncEnumerable<UniswapLiquidityPosition> SynchronizeAsync(UniswapChainConfiguration chain,
        Wallet wallet,
        IReadOnlyCollection<UniswapLiquidityPosition> dbPositions,
        DateOnly day,
        [EnumeratorCancellation] CancellationToken ct = default)
    {
        if (dbPositions.Count == 0)
        {
            yield break;
        }

        var dbPositionsMap = dbPositions.ToDictionary(x => x.PositionId);

        var uniswapPositions = await _uniswapProvider.GetPositionsAsync(chain, wallet);

        foreach (var uniswapPosition in uniswapPositions)
        {
            if (!dbPositionsMap.TryGetValue((ulong)uniswapPosition.PositionId, out var dbPosition))
            {
                continue;
            }

            var pool = await _uniswapProvider.GetPoolAsync(chain, uniswapPosition);

            var positionValuation = await _positionEvaluator.EvaluatePositionAsync(chain, uniswapPosition, pool, ct);

            var token0 = ToTokenWithFee(positionValuation.PositionTokens.Token0, positionValuation.PositionFees.Token0);
            var token1 = ToTokenWithFee(positionValuation.PositionTokens.Token1, positionValuation.PositionFees.Token1);

            dbPosition.AddOrUpdateSnapshot(day, positionValuation.IsInRange, token0, token1);

            yield return dbPosition;
        }
    }

    private static CryptoTokenStatisticWithFee ToTokenWithFee(
        CryptoToken token,
        CryptoToken fee)
    {
        return new CryptoTokenStatisticWithFee
        {
            Amount = token.Amount,
            PriceInUsd = token.PriceInUsd,
            Fee = fee.Amount
        };
    }
}