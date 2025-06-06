using System.Net.Http.Json;
using HyperliquidClient.UserNonFundingLedgerUpdates.Contracts;

namespace HyperliquidClient.UserNonFundingLedgerUpdates;

public interface IUserNonFundingLedgerUpdatesClient
{
    Task<UserNonFundingLedgerUpdate[]> GetUserNonFundingLedgerUpdates(string user,
        CancellationToken ct = default);
}

public class UserNonFundingLedgerUpdatesClient : IUserNonFundingLedgerUpdatesClient
{
    private readonly HttpClient _client;

    public UserNonFundingLedgerUpdatesClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<UserNonFundingLedgerUpdate[]> GetUserNonFundingLedgerUpdates(string user,
        CancellationToken ct = default)
    {
        using var response = await _client.PostAsJsonAsync("info",
            new GetUserNonFundingLedgerUpdatesRequest("userNonFundingLedgerUpdates", user), cancellationToken: ct);

        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<UserNonFundingLedgerUpdate[]>(cancellationToken: ct))!;
    }
}