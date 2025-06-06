namespace CryptoWatcher.Entities;

public class PositionInLiquidityPool
{
    public DateOnly CreatedDate { get; set; }
    
    
    public string WalletAddress { get; set; } = null!;
}