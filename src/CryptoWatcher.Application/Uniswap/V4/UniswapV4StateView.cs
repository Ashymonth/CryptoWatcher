using System.Numerics;
using CryptoWatcher.Application.Uniswap.V4;
using CryptoWatcher.Entities;
using Nethereum.ABI;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using UniswapClient.UniswapV4;

namespace CryptoWatcher.Host.Services.Uniswap.V4;

public class UniswapV4StateView
{
    private const string StateViewAddress = "0x86e8631a016f9068c3f085faf484ee3f5fdee8f2";

    private const string Abi = """
                               [
                               {
                                 "inputs": [
                                   { "internalType": "PoolId", "name": "poolId", "type": "bytes32" }
                                 ],
                                 "name": "getSlot0",
                                 "outputs": [
                                   { "internalType": "uint160", "name": "sqrtPriceX96", "type": "uint160" },
                                   { "internalType": "int24", "name": "tick", "type": "int24" },
                                   { "internalType": "uint24", "name": "protocolFee", "type": "uint24" },
                                   { "internalType": "uint24", "name": "lpFee", "type": "uint24" }
                                 ],
                                 "stateMutability": "view",
                                 "type": "function"
                               },
                               {
                                  "inputs": [
                                    { "internalType": "PoolId", "name": "poolId", "type": "bytes32" }
                                  ],
                                  "name": "getFeeGrowthGlobals",
                                  "outputs": [
                                    {
                                      "internalType": "uint256",
                                      "name": "feeGrowthGlobal0",
                                      "type": "uint256"
                                    },
                                    {
                                      "internalType": "uint256",
                                      "name": "feeGrowthGlobal1",
                                      "type": "uint256"
                                    }
                                  ],
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
                               },
                               {
                                  "inputs": [
                                    { "internalType": "PoolId", "name": "poolId", "type": "bytes32" },
                                    { "internalType": "int24", "name": "tick", "type": "int24" }
                                  ],
                                  "name": "getTickInfo",
                                  "outputs": [
                                    {
                                      "internalType": "uint128",
                                      "name": "liquidityGross",
                                      "type": "uint128"
                                    },
                                    { "internalType": "int128", "name": "liquidityNet", "type": "int128" },
                                    {
                                      "internalType": "uint256",
                                      "name": "feeGrowthOutside0X128",
                                      "type": "uint256"
                                    },
                                    {
                                      "internalType": "uint256",
                                      "name": "feeGrowthOutside1X128",
                                      "type": "uint256"
                                    }
                                  ],
                                  "stateMutability": "view",
                                  "type": "function"
                                },
                               {
                                 "inputs": [
                                   { "internalType": "PoolId", "name": "poolId", "type": "bytes32" },
                                   { "internalType": "address", "name": "owner", "type": "address" },
                                   { "internalType": "int24", "name": "tickLower", "type": "int24" },
                                   { "internalType": "int24", "name": "tickUpper", "type": "int24" },
                                   { "internalType": "bytes32", "name": "salt", "type": "bytes32" }
                                 ],
                                 "name": "getPositionInfo",
                                 "outputs": [
                                   { "internalType": "uint128", "name": "liquidity", "type": "uint128" },
                                   {
                                     "internalType": "uint256",
                                     "name": "feeGrowthInside0LastX128",
                                     "type": "uint256"
                                   },
                                   {
                                     "internalType": "uint256",
                                     "name": "feeGrowthInside1LastX128",
                                     "type": "uint256"
                                   }
                                 ],
                                 "stateMutability": "view",
                                 "type": "function"
                               }
                               ]
                               """;

    public async Task<GetSlot0OutputDTO> GetSlot0Async(IWeb3 web3,
        UniswapV4PoolKey poolId25)
    {
        var contract = web3.Eth.GetContract(Abi, StateViewAddress);

        var poolKey = GeneratePoolId(poolId25);

        var slot0 = await contract.GetFunction("getSlot0")
            .CallDeserializingToObjectAsync<GetSlot0OutputDTO>(poolKey);

        return slot0;
    }

    public async Task<GetTickFeeGrowthOutsideOutput> GetTickInfoAsync(IWeb3 web3, UniswapV4PoolKey poolId25, int tick)
    {
        var contract = web3.Eth.GetContract(Abi, StateViewAddress);

        var poolKey = GeneratePoolId(poolId25);

        return await contract.GetFunction("getTickInfo")
            .CallDeserializingToObjectAsync<GetTickFeeGrowthOutsideOutput>(poolKey, tick);
    }

    public async Task<GetPositionInfoOutputDTO> GetPositionInfoAsync(IWeb3 web3, UniswapV4PoolKey poolId25,
        Network network, int tickLower, int tickUpper, ulong tokenId)
    {
        var contract = web3.Eth.GetContract(Abi, StateViewAddress);

        var poolId = GeneratePoolId(poolId25);

        return await contract.GetFunction("getPositionInfo")
            .CallDeserializingToObjectAsync<GetPositionInfoOutputDTO>(poolId,
                "0x4529a01c7a0410167c5740c487a8de60232617bf", tickLower, tickUpper,
                ConvertTokenIdToBytes32(tokenId));
    }

    public async Task<GetFeeGrowthGlobalsOutput> GetFeeGrowGlobalAsync(IWeb3 web3, UniswapV4PoolKey poolId25)
    {
        var contract = web3.Eth.GetContract(Abi, StateViewAddress);

        var poolKey = GeneratePoolId(poolId25);

        return await contract.GetFunction("getFeeGrowthGlobals")
            .CallDeserializingToObjectAsync<GetFeeGrowthGlobalsOutput>(poolKey);
    }

    public async Task<UniswapV4PositionFetcher.FeeGrowthInsideOutputDTO> GetFeeGrowInside(
        IWeb3 web3,
        UniswapV4PoolKey poolKey,
        int tickLower,
        int tickUpper)
    {
        var contract = web3.Eth.GetContract(Abi, StateViewAddress);

        var feeGrowth = await contract.GetFunction("getFeeGrowthInside")
            .CallDeserializingToObjectAsync<UniswapV4PositionFetcher.FeeGrowthInsideOutputDTO>(
                GeneratePoolId(poolKey),
                tickLower,
                tickUpper);

        return feeGrowth;
    }

    private static byte[] GeneratePoolId(UniswapV4PoolKey poolKey)
    {
        var abiEncode = new ABIEncode();

        return abiEncode.GetSha3ABIEncoded(
            new ABIValue("address", poolKey.Currency0),
            new ABIValue("address", poolKey.Currency1),
            new ABIValue("uint24", poolKey.Fee),
            new ABIValue("int24", poolKey.TickSpacing),
            new ABIValue("address", poolKey.Hooks)
        );
    }

    public static byte[] ConvertTokenIdToBytes32(BigInteger tokenId)
    {
        byte[] bytes = tokenId.ToByteArray(isUnsigned: true, isBigEndian: true);
        byte[] result = new byte[32];

        Buffer.BlockCopy(
            src: bytes,
            srcOffset: 0,
            dst: result,
            dstOffset: 32 - bytes.Length,
            count: bytes.Length
        );

        return result;
    }

    private static byte[] GeneratePositionId(string owner, int tickLower, int tickUpper, BigInteger tokenId)
    {
        var abiEncode = new ABIEncode();
        return abiEncode.GetSha3ABIEncoded(
            new ABIValue("address", "0x4529a01c7a0410167c5740c487a8de60232617bf"), // position manager
            new ABIValue("int24", tickLower), // int24
            new ABIValue("int24", tickUpper), // int24
            new ABIValue("bytes32", tokenId));
    }

    [FunctionOutput]
    public class GetPositionInfoOutputDTO : IFunctionOutputDTO
    {
        [Parameter("uint128", "liquidity", 1)] public BigInteger positinoInfo { get; set; }

        [Parameter("uint256", "feeGrowthInside0LastX128", 2)]
        public BigInteger FeeGrowthInside0LastX128 { get; set; }

        [Parameter("uint256", "feeGrowthInside1LastX128", 3)]
        public BigInteger FeeGrowthInside1LastX128 { get; set; }
    }

    [FunctionOutput]
    public class GetSlot0OutputDTO : IFunctionOutputDTO
    {
        [Parameter("uint160", "sqrtPriceX96", 1)]
        public BigInteger SqrtPriceX96 { get; set; }

        [Parameter("int24", "tick", 2)] public int Tick { get; set; }

        [Parameter("uint24", "protocolFee", 3)]
        public uint ProtocolFee { get; set; }

        [Parameter("uint24", "lpFee", 4)] public uint LpFee { get; set; }
    }

    [FunctionOutput]
    public class GetFeeGrowthGlobalsOutput : IFunctionOutputDTO
    {
        [Parameter("uint256", "feeGrowthGlobal0", 1)]
        public BigInteger FeeGrowthGlobal0 { get; set; }

        [Parameter("uint256", "feeGrowthGlobal1", 2)]
        public BigInteger FeeGrowthGlobal1 { get; set; }
    }

    [FunctionOutput]
    public class GetTickFeeGrowthOutsideOutput : IFunctionOutputDTO
    {
        [Parameter("uint128", "liquidityGross", 1)]
        public BigInteger LiquidityGross { get; set; }

        [Parameter("int128", "liquidityNet", 2)]
        public BigInteger LiquidityNet { get; set; }

        [Parameter("uint256", "feeGrowthOutside0X128", 3)]
        public BigInteger FeeGrowthOutside0X128 { get; set; }

        [Parameter("uint256", "feeGrowthOutside1X128", 4)]
        public BigInteger FeeGrowthOutside1X128 { get; set; }
    }
}