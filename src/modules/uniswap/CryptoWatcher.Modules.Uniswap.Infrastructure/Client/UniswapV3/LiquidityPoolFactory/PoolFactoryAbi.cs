using System.Diagnostics.CodeAnalysis;

namespace CryptoWatcher.Modules.Uniswap.Infrastructure.Client.UniswapV3.LiquidityPoolFactory;

internal static class PoolFactoryAbi
{
    [StringSyntax(StringSyntaxAttribute.Json)]
    public const string Abi = """
                              [
                               {
                                "inputs": [
                                  { "internalType": "address", "name": "", "type": "address" },
                                  { "internalType": "address", "name": "", "type": "address" },
                                  { "internalType": "uint24", "name": "", "type": "uint24" }
                                ],
                                "name": "getPool",
                                "outputs": [{ "internalType": "address", "name": "", "type": "address" }],
                                "stateMutability": "view",
                                "type": "function"
                                }
                              ]
                              """;
}