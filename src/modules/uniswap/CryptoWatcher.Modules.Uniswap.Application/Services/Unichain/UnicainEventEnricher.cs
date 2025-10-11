using System.Numerics;
using System.Runtime.CompilerServices;
using CryptoWatcher.Abstractions;
using CryptoWatcher.Modules.Uniswap.Abstractions;
using CryptoWatcher.Modules.Uniswap.Application.Abstractions;
using CryptoWatcher.Modules.Uniswap.Application.Extensions;
using CryptoWatcher.Modules.Uniswap.Entities;
using CryptoWatcher.Modules.Uniswap.Models;
using CryptoWatcher.Shared.ValueObjects;
using Microsoft.Extensions.Logging;

namespace CryptoWatcher.Modules.Uniswap.Application.Services.Unichain;

public class CashFlowEventMatcher : ICashFlowEventMatcher
{
    private readonly ILiquidityEventsProvider _liquidityEventsProvider;
    private readonly ITokenEnricher _tokenEnricher;
    private readonly ILogger<CashFlowEventMatcher> _logger;

    public CashFlowEventMatcher(ILiquidityEventsProvider liquidityEventsProvider, ITokenEnricher tokenEnricher,
        ILogger<CashFlowEventMatcher> logger)
    {
        _liquidityEventsProvider = liquidityEventsProvider;
        _tokenEnricher = tokenEnricher;
        _logger = logger;
    }

    public async IAsyncEnumerable<List<UniswapLiquidityPositionCashFlow>> FetchCashFlowEvents(
        UniswapChainConfiguration chainConfiguration,
        BigInteger fromBlock,
        BigInteger toBlock,
        [EnumeratorCancellation] CancellationToken ct = default)
    {
        await foreach (var events in _liquidityEventsProvider.FetchLiquidityPoolEvents(chainConfiguration,
                           fromBlock, toBlock, ct))
        {
            var result = new List<UniswapLiquidityPositionCashFlow>();

            foreach (var poolPositionEvent in events)
            {
                var enrichedTokenPair =
                    await _tokenEnricher.EnrichAsync(chainConfiguration.RpcUrl, poolPositionEvent.TokenPair, ct);

                var positionFromDb = chainConfiguration.LiquidityPoolPositions.SingleOrDefault(position =>
                {
                    if (!IsTickMatch(position, poolPositionEvent))
                    {
                        return false;
                    }

                    var normalizedPair = enrichedTokenPair.NormalizeToPositionOrder(position);

                    return IsSymbolMatch(position.Token0, normalizedPair.Token0) &&
                           IsSymbolMatch(position.Token1, normalizedPair.Token1);
                });

                if (positionFromDb is null)
                {
                    _logger.LogDebug("No match for event ticks {TickLower}-{TickUpper}",
                        poolPositionEvent.TickLower, poolPositionEvent.TickUpper);
                    continue;
                }

                var cashFlow = UniswapLiquidityPositionCashFlow.CreateFromEvent(poolPositionEvent.Event,
                    positionFromDb.PositionId, chainConfiguration.Name, poolPositionEvent.TransactionHash,
                    enrichedTokenPair, poolPositionEvent.TimeStamp);

                result.Add(cashFlow);

                _logger.LogInformation("Matched cash flow for position {PositionId}", positionFromDb.PositionId);
            }

            yield return result;
        }
    }

    private static bool IsTickMatch(UniswapLiquidityPosition position, LiquidityPoolPositionEvent positionEvent)
    {
        return position.TickLower == positionEvent.TickLower &&
               position.TickUpper == positionEvent.TickUpper;
    }

    private static bool IsSymbolMatch(TokenInfo first, TokenInfo second)
    {
        return string.Equals(first.Symbol, second.Symbol, StringComparison.OrdinalIgnoreCase);
    }
}