namespace CryptoWatcher.Abstractions.CacheFlows;

public interface ICashFlow
{
    DateTimeOffset Date { get; }

    CashFlowEvent Event { get; }
}