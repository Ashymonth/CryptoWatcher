using System.Diagnostics.CodeAnalysis;

namespace AaveClient.PoolAddressesProvider;

internal class PoolAddressesProviderAbi
{
    [StringSyntax(StringSyntaxAttribute.Json)]
    public const string Abi = """
                              [

                               {
                                "inputs": [],
                                "name": "getPool",
                                "outputs": [{ "internalType": "address", "name": "", "type": "address" }],
                                "stateMutability": "view",
                                "type": "function"
                              },
                              {
                                 "inputs": [],
                                 "name": "getPriceOracle",
                                 "outputs": [{ "internalType": "address", "name": "", "type": "address" }],
                                 "stateMutability": "view",
                                 "type": "function"
                               }
                              ]

                              """;
}