using System.Diagnostics.CodeAnalysis;

namespace AaveClient.UiPoolDataProvider;

internal static class UiPoolDataProviderFetcherAbi
{
    [StringSyntax(StringSyntaxAttribute.Json)]
    public const string Abi = """
                              [{
                                 "inputs": [
                                   {
                                     "internalType": "contract IPoolAddressesProvider",
                                     "name": "provider",
                                     "type": "address"
                                   },
                                   { "internalType": "address", "name": "user", "type": "address" }
                                 ],
                                 "name": "getUserReservesData",
                                 "outputs": [
                                   {
                                     "components": [
                                       {
                                         "internalType": "address",
                                         "name": "underlyingAsset",
                                         "type": "address"
                                       },
                                       {
                                         "internalType": "uint256",
                                         "name": "scaledATokenBalance",
                                         "type": "uint256"
                                       },
                                       {
                                         "internalType": "bool",
                                         "name": "usageAsCollateralEnabledOnUser",
                                         "type": "bool"
                                       },
                                       {
                                         "internalType": "uint256",
                                         "name": "scaledVariableDebt",
                                         "type": "uint256"
                                       }
                                     ],
                                     "internalType": "struct IUiPoolDataProviderV3.UserReserveData[]",
                                     "name": "",
                                     "type": "tuple[]"
                                   },
                                   { "internalType": "uint8", "name": "", "type": "uint8" }
                                 ],
                                 "stateMutability": "view",
                                 "type": "function"
                               }]
                              """;
}