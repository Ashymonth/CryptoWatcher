using CryptoWatcher.Abstractions;
using CryptoWatcher.Extensions;
using CryptoWatcher.Modules.Merkl.Application.Abstractions;
using CryptoWatcher.Modules.Merkl.Entities;
using CryptoWatcher.Modules.Merkl.Specifications;
using CryptoWatcher.Modules.Merkl.ValueObjects;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Merkl.Application.Services;

public class MerklSyncService : IMerklSyncService
{
    private readonly IMerklProvider _provider;
    private readonly IRepository<MerklCampaign> _campaignRepo;
    private readonly IRepository<MerklCampaignSnapshot> _snapshotsRepo;
    private readonly IRepository<MerklCampaignCashFlow> _cashFlowRepo;

    public MerklSyncService(IMerklProvider provider, IRepository<MerklCampaign> campaignRepo,
        IRepository<MerklCampaignSnapshot> snapshotsRepo, IRepository<MerklCampaignCashFlow> cashFlowRepo)
    {
        _provider = provider;
        _campaignRepo = campaignRepo;
        _snapshotsRepo = snapshotsRepo;
        _cashFlowRepo = cashFlowRepo;
    }

    public async Task SyncRewardsAsync(EvmAddress walletAddress, int chainId,
        DateOnly day,
        CancellationToken ct = default)
    {
        var rewards = await _provider.GetUserRewardsAsync(walletAddress, chainId, ct);

        var dbCampaigns = (await _campaignRepo.ListAsync(new GetCampaignsWithSnapshotsSpecification(walletAddress), ct))
            .ToDictionary(campaign => campaign.Reason);

        var result = new List<MerklCampaign>();

        foreach (var campaignInfo in
                 rewards.GroupBy(info => new { info.Reason, info.Asset.Symbol, info.Asset.Address }))
        {
            if (!dbCampaigns.TryGetValue(campaignInfo.Key.Reason, out var campaign))
            {
                campaign = new MerklCampaign(chainId, walletAddress, campaignInfo.Key.Reason, campaignInfo.Key.Symbol,
                    campaignInfo.Key.Address);
            }

            var rewardStatus = new RewardStatus();
            foreach (var rewardCampaignGroup in campaignInfo)
            {
                rewardStatus.PendingAmount += rewardCampaignGroup.Pending.ToDecimal(rewardCampaignGroup.Asset.Decimals);
                rewardStatus.ClaimedAmount += rewardCampaignGroup.Claimed.ToDecimal(rewardCampaignGroup.Asset.Decimals);
                rewardStatus.ClaimabelAmount +=
                    rewardCampaignGroup.Amount.ToDecimal(rewardCampaignGroup.Asset.Decimals);
            }

            if (rewardStatus.ClaimabelAmount - rewardStatus.ClaimedAmount == 0 && rewardStatus.PendingAmount == 0)
            {
                continue;
            }

            var asset = campaignInfo.First().Asset;
            campaign.AddOrUpdateSnapshot(day, rewardStatus, asset.PriceInUsd);
            result.Add(campaign);
        }

        await _campaignRepo.UnitOfWork.BeginTransactionAsync(ct);
        try
        {
            await _campaignRepo.BulkMergeAsync(result, ct);

            var snapshots = result.SelectMany(campaign => campaign.Snapshots).ToArray();

            await _snapshotsRepo.BulkMergeAsync(snapshots, ct);

            var cashFlows = result.SelectMany(campaign => campaign.CashFlows).ToArray();

            await _cashFlowRepo.BulkMergeAsync(cashFlows, ct);

            await _campaignRepo.UnitOfWork.CommitTransactionAsync(ct);
        }
        catch (Exception e)
        {
            await _campaignRepo.UnitOfWork.RollbackTransactionAsync(ct);
            throw;
        }
    }
}