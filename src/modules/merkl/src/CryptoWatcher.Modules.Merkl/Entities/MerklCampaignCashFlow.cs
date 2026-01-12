using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Merkl.Entities;

public class MerklCampaignCashFlow
{
    private MerklCampaignCashFlow()
    {
        
    }
    
    public MerklCampaignCashFlow(Guid merklCampaignId, DateTime claimDate, CryptoTokenStatistic claimedAmount)
    {
        Id = Guid.CreateVersion7();
        MerklCampaignId = merklCampaignId;
        ClaimDate = claimDate;
        ClaimedAmount = claimedAmount;
    }

    public Guid Id { get; private set; }
    
    public Guid MerklCampaignId { get; private set; }

    public DateTime ClaimDate { get; private set; }

    public CryptoTokenStatistic ClaimedAmount { get; private set; } = null!;
}