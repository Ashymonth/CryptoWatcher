using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;

namespace CryptoWatcher.Application.Uniswap.V3;

public class UniswapV3PoolFactory
{
    private const string PoolFactoryAbi = """
                                          [
                                            {
                                            "inputs": [
                                              { "internalType": "address", "name": "tokenA", "type": "address" },
                                              { "internalType": "address", "name": "tokenB", "type": "address" }
                                            ],
                                            "name": "getPairPools",
                                            "outputs": [
                                              {
                                                "components": [
                                                  { "internalType": "address", "name": "pool", "type": "address" },
                                                  { "internalType": "int24", "name": "tickSpacing", "type": "int24" }
                                                ],
                                                "internalType": "struct SyncSwapRangePoolFactoryZKSync.PoolInfo[]",
                                                "name": "",
                                                "type": "tuple[]"
                                              }
                                            ],
                                            "stateMutability": "view",
                                            "type": "function"
                                            }
                                          ]
                                          """;

    public async Task<string> GetPoolAddressAsync(IWeb3 web3, string poolFactoryAddress, string token0, string token1)
    {
        var contract = web3.Eth.GetContract(PoolFactoryAbi, poolFactoryAddress);
        var function = contract.GetFunction("getPairPools");

        var result = await function.CallDeserializingToObjectAsync<GetPairPoolsOutputDto>(token0, token1);
        return result.Pools.First().Pool;
    }

    [FunctionOutput]
    private class GetPairPoolsOutputDto : IFunctionOutputDTO
    {
        [Parameter("tuple[]", "", 1)] public List<PoolInfo> Pools { get; set; } = null!;
    }

    // ReSharper disable once ClassNeverInstantiated.Local
    private class PoolInfo
    {
        [Parameter("address", "pool", 1)] public string Pool { get; set; } = null!;
    }
}