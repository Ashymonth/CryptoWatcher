using System.Numerics;
using CryptoWatcher.Application.Uniswap;
using CryptoWatcher.Entities;
using CryptoWatcher.Extensions;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Web3;
using UniswapClient.Models;
using UniswapClient.UniswapV3;

namespace CryptoWatcher.Host.Services.Uniswap.V3;

public class UniswapV3PositionFetcher : UniswapPositionFetcherBase
{
    protected override async Task<List<IUniswapPosition>> GetPositionsDataAsync(IWeb3 web3, Network network,
        Wallet wallet, BigInteger balance)
    {
        var tokenIds = await GetTokenIdsAsync(web3, network, wallet, balance);
        return await GetPositionsDataAsync(web3, network, tokenIds);
    }

    private static async Task<List<ulong>> GetTokenIdsAsync(IWeb3 web3, Network network, Wallet wallet,
        BigInteger count)
    {
        var calls = Enumerable.Range(0, (int)count).Select(i => new Call
        {
            Target = network.NftManagerAddress, CallData = new TokenOfOwnerByIndexFunction
            {
                Owner = wallet.Address,
                Index = i
            }.GetCallData()
        }).ToList();

        return await web3.MultiCallAsync(calls, network.MultiCallAddress,
            bytes =>
            {
                var response = new TokenOfOwnerByIndexOutputDTO().DecodeOutput(bytes.ToHex());
                return (ulong)response.TokenId;
            });
    }

    private static async Task<List<IUniswapPosition>> GetPositionsDataAsync(IWeb3 web3, Network network,
        List<ulong> tokenIds)
    {
        var calls = tokenIds.Select(tokenId => new Call
            {
                Target = network.NftManagerAddress, CallData = new PositionsFunction { TokenId = tokenId }.GetCallData()
            })
            .ToList();

        var result = await web3.MultiCallAsync(calls, network.MultiCallAddress,
            bytes => new PositionsOutputDTO().DecodeOutput(bytes.ToHex()));

        return result.Select((output, i) => new UniswapV3PositionInfo
            {
                Token0 = output.Token0,
                Token1 = output.Token1,
                TickLower = output.TickLower,
                TickUpper = output.TickUpper,
                Liquidity = output.Liquidity,
                FeeGrowthInside0LastX128 = output.FeeGrowthInside0LastX128,
                FeeGrowthInside1LastX128 = output.FeeGrowthInside1LastX128,
                PositionId = tokenIds[i]
            })
            .Cast<IUniswapPosition>()
            .ToList();
    }

    [Function("tokenOfOwnerByIndex", "uint256")]
    public class TokenOfOwnerByIndexFunction : FunctionMessage
    {
        [Parameter("address", "_owner", 1)] public string Owner { get; set; }
        [Parameter("uint256", "_index", 2)] public BigInteger Index { get; set; }
    }

    [Function("positions", typeof(PositionsOutputDTO))]
    public class PositionsFunction : FunctionMessage
    {
        [Parameter("uint256", "tokenId", 1)] public BigInteger TokenId { get; set; }
    }

    // Output DTOs
    [FunctionOutput]
    public class TokenOfOwnerByIndexOutputDTO : IFunctionOutputDTO
    {
        [Parameter("uint256", "tokenId", 1)] public BigInteger TokenId { get; set; }
    }

    [FunctionOutput]
    public class PositionsOutputDTO : IFunctionOutputDTO
    {
        [Parameter("uint96", "nonce", 1)] public BigInteger Nonce { get; set; }
        [Parameter("address", "operator", 2)] public string Operator { get; set; }
        [Parameter("address", "token0", 3)] public string Token0 { get; set; }
        [Parameter("address", "token1", 4)] public string Token1 { get; set; }
        [Parameter("uint24", "fee", 5)] public uint Fee { get; set; }
        [Parameter("int24", "tickLower", 6)] public int TickLower { get; set; }
        [Parameter("int24", "tickUpper", 7)] public int TickUpper { get; set; }
        [Parameter("uint128", "liquidity", 8)] public BigInteger Liquidity { get; set; }

        [Parameter("uint256", "feeGrowthInside0LastX128", 9)]
        public BigInteger FeeGrowthInside0LastX128 { get; set; }

        [Parameter("uint256", "feeGrowthInside1LastX128", 10)]
        public BigInteger FeeGrowthInside1LastX128 { get; set; }
    }
}