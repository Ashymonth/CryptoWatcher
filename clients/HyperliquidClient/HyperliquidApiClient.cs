using HyperliquidClient.UserNonFundingLedgerUpdates;
using HyperliquidClient.UserVaultEquities;

namespace HyperliquidClient;

public interface IHyperliquidApiClient
{
    IUserNonFundingLedgerUpdatesClient UserNonFundingLedgerUpdates { get; }

    IUserVaultEquitiesClient UserVaultEquities { get; }
}

public class HyperliquidApiClient : IHyperliquidApiClient
{
    public HyperliquidApiClient(IUserVaultEquitiesClient userVaultEquities,
        IUserNonFundingLedgerUpdatesClient userNonFundingLedgerUpdates)
    {
        UserVaultEquities = userVaultEquities;
        UserNonFundingLedgerUpdates = userNonFundingLedgerUpdates;
    }

    public IUserNonFundingLedgerUpdatesClient UserNonFundingLedgerUpdates { get; }

    public IUserVaultEquitiesClient UserVaultEquities { get; }
}