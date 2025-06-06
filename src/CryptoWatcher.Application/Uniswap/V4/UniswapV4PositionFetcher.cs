using System.Globalization;
using System.Numerics;
using CryptoWatcher.Entities;
using CryptoWatcher.Host.Services.Uniswap.V4;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using UniswapClient.Models;
using UniswapClient.UniswapV4;

namespace CryptoWatcher.Application.Uniswap.V4;

public class UniswapV4PositionFetcher : UniswapPositionFetcherBase
{
    private readonly UniswapV4ClientOld _client;
    private readonly UniswapV4StateView _stateView;
    private readonly UniswapV4Client _uniswapV4Client;

    public UniswapV4PositionFetcher(UniswapV4ClientOld client, UniswapV4StateView stateView,
        UniswapV4Client uniswapV4Client)
    {
        _client = client;
        _stateView = stateView;
        _uniswapV4Client = uniswapV4Client;
    }

    private const string Abi = """
                               [
                               {
                                 "inputs": [
                                   { "internalType": "uint256", "name": "tokenId", "type": "uint256" }
                                 ],
                                 "name": "getPositionLiquidity",
                                 "outputs": [
                                   { "internalType": "uint128", "name": "liquidity", "type": "uint128" }
                                 ],
                                 "stateMutability": "view",
                                 "type": "function"
                               },
                               {
                                  "inputs": [
                                    { "internalType": "uint256", "name": "tokenId", "type": "uint256" }
                                  ],
                                  "name": "getPoolAndPositionInfo",
                                  "outputs": [
                                    {
                                      "components": [
                                        {
                                          "internalType": "Currency",
                                          "name": "currency0",
                                          "type": "address"
                                        },
                                        {
                                          "internalType": "Currency",
                                          "name": "currency1",
                                          "type": "address"
                                        },
                                        { "internalType": "uint24", "name": "fee", "type": "uint24" },
                                        { "internalType": "int24", "name": "tickSpacing", "type": "int24" },
                                        {
                                          "internalType": "contract IHooks",
                                          "name": "hooks",
                                          "type": "address"
                                        }
                                      ],
                                      "internalType": "struct PoolKey",
                                      "name": "poolKey",
                                      "type": "tuple"
                                    },
                                    { "internalType": "PositionInfo", "name": "info", "type": "uint256" }
                                  ],
                                  "stateMutability": "view",
                                  "type": "function"
                                },
                                {
                                  "inputs": [
                                    { "internalType": "bytes32", "name": "slot", "type": "bytes32" }
                                  ],
                                  "name": "extsload",
                                  "outputs": [{ "internalType": "bytes32", "name": "", "type": "bytes32" }],
                                  "stateMutability": "view",
                                  "type": "function"
                                },
                                {
                                 "inputs": [
                                   { "internalType": "PoolId", "name": "poolId", "type": "bytes32" },
                                   { "internalType": "int24", "name": "tickLower", "type": "int24" },
                                   { "internalType": "int24", "name": "tickUpper", "type": "int24" }
                                 ],
                                 "name": "getFeeGrowthInside",
                                 "outputs": [
                                   {
                                     "internalType": "uint256",
                                     "name": "feeGrowthInside0X128",
                                     "type": "uint256"
                                   },
                                   {
                                     "internalType": "uint256",
                                     "name": "feeGrowthInside1X128",
                                     "type": "uint256"
                                   }
                                 ],
                                 "stateMutability": "view",
                                 "type": "function"
                               }
                                ]
                               """;

    protected override async Task<List<IUniswapPosition>> GetPositionsDataAsync(IWeb3 web3, Network network,
        Wallet wallet, BigInteger balance)
    {
        return await _uniswapV4Client.PositionFetcher.GetPositionsDataAsync(web3,
            new NetworkInfo
            {
                NetworkUrl = network.RpcUrl,
                MultiCallAddress = network.MultiCallAddress,
                NftManagerAddress = network.NftManagerAddress
            }
            , wallet.Address);

        var tokenIds = await _client.GetPoolPositionTokenIdsAsync(wallet.Address);

        return await GetPositionsDataAsync(web3, network, tokenIds);
    }

    private async Task<List<IUniswapPosition>> GetPositionsDataAsync(IWeb3 web3,
        Network network,
        IReadOnlyCollection<ulong> tokenIds)
    {
        var result = new List<IUniswapPosition>();
        foreach (var tokenId in tokenIds)
        {
            var contract = web3.Eth.GetContract(Abi, network.NftManagerAddress);
            var packedData = await contract.GetFunction("getPoolAndPositionInfo")
                .CallDeserializingToObjectAsync<GetPoolAndPositionInfoOutputDTO>(tokenId);

            var positionInfo = PositionInfoParser.FromUInt256(packedData.PositionInfo);

            var poolKey = new UniswapV4PoolKey
            {
                Currency0 = packedData.PoolKey.Currency0,
                Currency1 = packedData.PoolKey.Currency1,
                Fee = packedData.PoolKey.Fee,
                TickSpacing = packedData.PoolKey.TickSpacing,
                Hooks = packedData.PoolKey.Hooks,
            };

            var feeGrowth = await _stateView.GetPositionInfoAsync(web3,  poolKey, network, positionInfo.TickLower,
                positionInfo.TickUpper, tokenId);

            result.Add(new UniswapV4PositionInfo
            {
                PoolKey = poolKey,
                PositionId = tokenId,
                TickLower = positionInfo.TickLower,
                TickUpper = positionInfo.TickUpper,
                Token0 = packedData.PoolKey.Currency0,
                Token1 = packedData.PoolKey.Currency1,
                Liquidity = feeGrowth.positinoInfo,
                FeeGrowthInside0LastX128 = feeGrowth.FeeGrowthInside0LastX128,
                FeeGrowthInside1LastX128 = feeGrowth.FeeGrowthInside1LastX128
            });
        }

        return result;
    }

    [FunctionOutput]
    public class FeeGrowthInsideOutputDTO : IFunctionOutputDTO
    {
        [Parameter("uint256", "feeGrowthInside0X128", 1)]
        public BigInteger FeeGrowthInside0X128 { get; set; }

        [Parameter("uint256", "feeGrowthInside1X128", 2)]
        public BigInteger FeeGrowthInside1X128 { get; set; }
    }

    [FunctionOutput]
    public class GetPoolAndPositionInfoOutputDTO : IFunctionOutputDTO
    {
        [Parameter("tuple", "poolKey", 1)] public PoolKey PoolKey { get; set; }

        [Parameter("uint256", "info", 2)] public BigInteger PositionInfo { get; set; }
    }

    public class PoolKey
    {
        [Parameter("address", "currency0", 1)] public string Currency0 { get; set; }

        [Parameter("address", "currency1", 2)] public string Currency1 { get; set; }

        [Parameter("uint24", "fee", 3)] public uint Fee { get; set; }

        [Parameter("int24", "tickSpacing", 4)] public int TickSpacing { get; set; }

        [Parameter("address", "hooks", 5)] public string Hooks { get; set; }
    }


    private struct PositionInfo
    {
        public int TickLower;
        public int TickUpper;
    }

    private static class PositionInfoParser
    {
        private static readonly BigInteger MaskUpper200Bits = BigInteger.Parse(
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000000000",
            NumberStyles.HexNumber);

        private const uint Mask24Bits = 0xFFFFFF;

        private const int TickLowerOffset = 8;
        private const int TickUpperOffset = 32;

        public static PositionInfo FromUInt256(BigInteger value)
        {
            // tickLower = 24 бита начиная с 8 позиции
            int tickLower = (int)((value >> TickLowerOffset) & Mask24Bits);
            // приведение знака (поскольку int24)
            if ((tickLower & (1 << 23)) != 0)
                tickLower |= unchecked((int)0xFF000000);

            // tickUpper = 24 бита начиная с 32 позиции
            int tickUpper = (int)((value >> TickUpperOffset) & Mask24Bits);
            if ((tickUpper & (1 << 23)) != 0)
                tickUpper |= unchecked((int)0xFF000000);

            // poolId = верхние 200 бит
            BigInteger poolIdBig = value & MaskUpper200Bits;
            byte[] poolIdBytes = new byte[25];
            byte[] poolIdFullBytes = poolIdBig.ToByteArray(isUnsigned: true, isBigEndian: true);

            // poolIdFullBytes может быть короче 32 байт (если старшие нули), дополним до 32 перед срезкой
            if (poolIdFullBytes.Length < 32)
            {
                var temp = new byte[32];
                Array.Copy(poolIdFullBytes, 0, temp, 32 - poolIdFullBytes.Length, poolIdFullBytes.Length);
                poolIdFullBytes = temp;
            }

            Array.Copy(poolIdFullBytes, 0, poolIdBytes, 0, 25);

            return new PositionInfo
            {
                TickLower = tickLower,
                TickUpper = tickUpper,
            };
        }
    }
}