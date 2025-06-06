namespace CryptoWatcher.Entities;

public class LendingPosition
{
    public string WalletAddress { get; init; } = null!; 
    
    public Wallet Wallet { get; set; } = null!;
}