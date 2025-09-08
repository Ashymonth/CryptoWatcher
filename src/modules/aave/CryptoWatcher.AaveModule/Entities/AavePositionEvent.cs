namespace CryptoWatcher.AaveModule.Entities;

public class AavePositionEvent
{
    public Guid PositionId { get; set; }
    
    public decimal Amount { get; set; }

    public DateOnly Date { get; set; }
    
    public AavePositionEventType EventType { get; set; }
}