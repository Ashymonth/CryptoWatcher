using System.Numerics;
using CryptoWatcher.Modules.Uniswap.Entities;
using CryptoWatcher.Modules.Uniswap.Infrastructure.Client.UniswapV4.StateView.Contracts;
using CryptoWatcher.Modules.Uniswap.Infrastructure.Services;
using Nethereum.ABI;
using Nethereum.Web3;

namespace CryptoWatcher.Modules.Uniswap.Infrastructure.Client.UniswapV4.StateView;

internal interface IUniswapV4StateView
{
    Task<GetSlot0OutputDTO> GetSlot0Async(UniswapChainConfiguration chain, UniswapV4PoolKey poolId25);

    Task<GetTickFeeGrowthOutsideOutput> GetTickInfoAsync(UniswapChainConfiguration chain, UniswapV4PoolKey poolId25,
        int tick);

    Task<GetPositionInfoOutputDTO> GetPositionInfoAsync(UniswapChainConfiguration chain, UniswapV4PoolKey poolId25,
        int tickLower, int tickUpper, ulong tokenId);

    Task<GetFeeGrowthGlobalsOutput> GetFeeGrowGlobalAsync(UniswapChainConfiguration chain, UniswapV4PoolKey poolId25);
}

internal class UniswapV4StateView : IUniswapV4StateView
{
    private readonly IWeb3Factory _web3Factory;

    public UniswapV4StateView(IWeb3Factory web3Factory)
    {
        _web3Factory = web3Factory;
    }

    public async Task<GetSlot0OutputDTO> GetSlot0Async(UniswapChainConfiguration chain, UniswapV4PoolKey poolId25)
    {
        var web3 = _web3Factory.GetWeb3(chain);
        var contract = web3.Eth.GetContract(UniswapV4StateViewAbi.Abi, "0x76Fd297e2D437cd7f76d50F01AfE6160f86e9990");

        var poolKey = GeneratePoolId(poolId25);

        var slot0 = await contract.GetFunction("getSlot0")
            .CallDeserializingToObjectAsync<GetSlot0OutputDTO>(poolKey);

        return slot0;
    }

    public async Task<GetTickFeeGrowthOutsideOutput> GetTickInfoAsync(UniswapChainConfiguration chain,
        UniswapV4PoolKey poolId25, int tick)
    {
        var web3 = _web3Factory.GetWeb3(chain);
        var contract = web3.Eth.GetContract(UniswapV4StateViewAbi.Abi, chain.SmartContractAddresses.StateView!);

        var poolKey = GeneratePoolId(poolId25);

        return await contract.GetFunction("getTickInfo")
            .CallDeserializingToObjectAsync<GetTickFeeGrowthOutsideOutput>(poolKey, tick);
    }

    public async Task<GetPositionInfoOutputDTO> GetPositionInfoAsync(UniswapChainConfiguration chain,
        UniswapV4PoolKey poolId25,
        int tickLower, int tickUpper, ulong tokenId)
    {
        var web3 = _web3Factory.GetWeb3(chain);
        var contract = web3.Eth.GetContract(UniswapV4StateViewAbi.Abi, chain.SmartContractAddresses.StateView!);

        var poolId = GeneratePoolId(poolId25);

        return await contract.GetFunction("getPositionInfo").CallDeserializingToObjectAsync<GetPositionInfoOutputDTO>(
            poolId, chain.SmartContractAddresses.PositionManager.Value, tickLower, tickUpper, ConvertTokenIdToBytes32(tokenId));
    }

    public async Task<GetFeeGrowthGlobalsOutput> GetFeeGrowGlobalAsync(UniswapChainConfiguration chain,
        UniswapV4PoolKey poolId25)
    {
        var web3 = _web3Factory.GetWeb3(chain);
        var contract = web3.Eth.GetContract(UniswapV4StateViewAbi.Abi, chain.SmartContractAddresses.StateView!);

        var poolKey = GeneratePoolId(poolId25);

        return await contract.GetFunction("getFeeGrowthGlobals")
            .CallDeserializingToObjectAsync<GetFeeGrowthGlobalsOutput>(poolKey);
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

    private static byte[] ConvertTokenIdToBytes32(BigInteger tokenId)
    {
        var bytes = tokenId.ToByteArray(isUnsigned: true, isBigEndian: true);
        var result = new byte[32];

        Buffer.BlockCopy(
            src: bytes,
            srcOffset: 0,
            dst: result,
            dstOffset: 32 - bytes.Length,
            count: bytes.Length
        );

        return result;
    }
}