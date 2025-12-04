using System.Numerics;
using Bogus;
using CryptoWatcher.Modules.Uniswap.Entities;
using CryptoWatcher.Modules.Uniswap.Models;
using CryptoWatcher.Modules.Uniswap.Tests.DataSets;
using CryptoWatcher.Shared.ValueObjects;

namespace CryptoWatcher.Modules.Uniswap.Tests.Fakers;

public sealed class LiquidityPoolPositionEventFaker : Faker<LiquidityPoolPositionEvent>
{
    public LiquidityPoolPositionEventFaker(UniswapLiquidityPosition position, BigInteger liquidityDelta,
        DateTime timestamp)
    {
        CustomInstantiator(faker => new LiquidityPoolPositionEvent
        {
            WalletAddress = position.WalletAddress,
            TransactionHash = faker.Crypto().TxHash(),
            TickLower = position.TickLower,
            TickUpper = position.TickUpper,
            LiquidityDelta = liquidityDelta,
            TimeStamp = timestamp,
            TokenPair = new TokenPair
            {
                Token0 = new Token { Address = faker.Crypto().EvmAddress(), Balance = faker.Random.ULong() },
                Token1 = new Token { Address = faker.Crypto().EvmAddress(), Balance = faker.Random.ULong() }
            }
        });
    }
}