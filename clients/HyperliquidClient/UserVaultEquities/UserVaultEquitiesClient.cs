using System.Net.Http.Json;
using HyperliquidClient.UserVaultEquities.Contracts;

namespace HyperliquidClient.UserVaultEquities;

public interface IUserVaultEquitiesClient
{
    Task<UserVaultEquity[]> GetUserVaultEquities(string user, CancellationToken ct = default);
}

public class UserVaultEquitiesClient : IUserVaultEquitiesClient
{
    private readonly HttpClient _client;

    public UserVaultEquitiesClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<UserVaultEquity[]> GetUserVaultEquities(string user, CancellationToken ct = default)
    {
        using var response = await _client.PostAsJsonAsync("info",
            new GetUserVaultEquitiesRequest("userVaultEquities", user), cancellationToken: ct);

        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<UserVaultEquity[]>(cancellationToken: ct))!;
    }
}