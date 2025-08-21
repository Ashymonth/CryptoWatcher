using System.Net.Http.Json;
using HyperliquidClient.UserNonFundingLedgerUpdates.Contracts;

namespace HyperliquidClient.UserNonFundingLedgerUpdates;

public interface IUserNonFundingLedgerUpdatesClient
{
    /// <summary>
    /// Retrieve a user's funding history or non-funding ledger updates
    /// </summary>
    /// <param name="user">user(wallet) address</param>
    /// <param name="ct"></param>
    /// <remarks>https://hyperliquid.gitbook.io/hyperliquid-docs/for-developers/api/info-endpoint/perpetuals#request-body-4</remarks>
    /// <returns></returns>
    Task<UserNonFundingLedgerUpdate[]> GetUserNonFundingLedgerUpdates(string user,
        CancellationToken ct = default);
}

/// <summary>
/// <inheritdoc cref="IUserNonFundingLedgerUpdatesClient"/>
/// </summary>
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