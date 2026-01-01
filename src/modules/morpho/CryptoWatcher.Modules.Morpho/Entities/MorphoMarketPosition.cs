using CryptoWatcher.Abstractions;
using CryptoWatcher.Exceptions;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Morpho.Entities;

/// <summary>
/// Represent market position from morpho.
/// Merket position contains borrowed asset and collateral token. 
/// </summary>
public class MorphoMarketPosition : IDeFiPosition<MorphoMarketPositionSnapshot, MorphoMarketPositionCashFlow>
{
    private readonly List<MorphoMarketPositionCashFlow> _cashFlows = [];

    private readonly List<MorphoMarketPositionSnapshot> _snapshots = [];

    internal const string PositionClosedException = "Position already closed";
    internal const string ClosedAtGreatestThatCreatedAtException = "ClosedAt can't be greatest that CreatedAt";

    public MorphoMarketPosition(
        Guid marketExternalMarketExternalId,
        int chainId,
        CryptoToken loadToken,
        CryptoToken collateralToken,
        DateTime createdAt)
    {
        MarketExternalId = marketExternalMarketExternalId;
        ChainId = chainId;
        LoanToken = loadToken;
        CollateralToken = collateralToken;
        CreatedAt = createdAt;
    }

    public Guid Id { get; set; }

    /// <summary>
    /// External id from morpho api.
    /// </summary>
    public Guid MarketExternalId { get; private set; }

    public int ChainId { get; private set; }

    public CryptoToken LoanToken { get; private set; }

    public CryptoToken CollateralToken { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? ClosedAt { get; private set; }

    public bool IsActive => ClosedAt is not null;

    public EvmAddress WalletAddress { get; set; }

    public IReadOnlyCollection<MorphoMarketPositionSnapshot> Snapshots => _snapshots;

    public IReadOnlyCollection<MorphoMarketPositionCashFlow> CashFlows => _cashFlows;

    public void AddSnapshot(DateOnly day, CryptoTokenStatistic load, CryptoTokenStatistic collateralToken,
        double healthFactor)
    {
        if (IsActive)
        {
            throw new DomainException(PositionClosedException);
        }

        var existedSnapshot = Snapshots.FirstOrDefault(snapshot => snapshot.Day == day);
        if (existedSnapshot is not null)
        {
            existedSnapshot.UpdateSnapshot(load, collateralToken, healthFactor);
            return;
        }

        _snapshots.Add(new MorphoMarketPositionSnapshot(MarketExternalId, day, load, collateralToken, healthFactor));
    }
    
    public void ClosePosition(DateTime closedAt)
    {
        if (closedAt > CreatedAt)
        {
            throw new DomainException(ClosedAtGreatestThatCreatedAtException);
        }

        if (!IsActive)
        {
            throw new DomainException(PositionClosedException);
        }

        ClosedAt = closedAt;
    }
}