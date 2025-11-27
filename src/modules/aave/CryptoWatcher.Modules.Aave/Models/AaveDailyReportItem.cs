using CryptoWatcher.Models;

namespace CryptoWatcher.Modules.Aave.Models;

public class AaveDailyReportItem : PlatformDailyReportItem
{
    public required string TokenSymbol { get; init; } = null!;
    
    public required string NetworkName { get; init; } = null!;
 
    public required decimal PositionGrowInUsd { get; init; }
    
    public required decimal PositionInToken { get; init; }
    
    public required decimal DailyProfitInToken { get; init; }
    
    public required decimal CashFlowsInUsd { get; init; } 
    
    public required decimal CashFlowsInToken { get; init; } 
  
}