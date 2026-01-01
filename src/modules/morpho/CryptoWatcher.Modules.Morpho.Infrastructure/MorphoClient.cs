using CryptoWatcher.Modules.Morpho.Infrastructure.MorphoApiClient.Contracts;
using GraphQL;
using GraphQL.Client.Abstractions;

namespace CryptoWatcher.Modules.Morpho.Infrastructure;

internal interface IMorphoClient
{
    Task<UserByAddressResponse?> GetUserMarketPositionsAsync(string address, int chainId, CancellationToken ct = default);
}

internal class MorphoClient : IMorphoClient
{
    private readonly IGraphQLClient _graphQlClient;

    public MorphoClient(IGraphQLClient graphQlClient)
    {
        _graphQlClient = graphQlClient;
    }
 
    public async Task<UserByAddressResponse?> GetUserMarketPositionsAsync(string address, int chainId,
        CancellationToken ct = default)
    {
        var userByAddressRequest = new GraphQLRequest
        {
            Query = UserByAddressQuery.Query,
            OperationName = "UserByAddress",
            Variables = new
            {
                address,
                chainId
            }
        };

      var result = await _graphQlClient.SendQueryAsync<UserByAddressResponse>(userByAddressRequest, ct);

      return result.Data;
    }
}