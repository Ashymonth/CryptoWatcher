using Ardalis.Specification;
using CryptoWatcher.Modules.Morpho.Entities;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Morpho.Specifications;

public sealed class MorphoMarketActivePositions : Specification<MorphoMarketPosition>
{
    public MorphoMarketActivePositions(EvmAddress walletAddress, DateOnly from, DateOnly to)
    {
        Query
            .Include(position => position.Snapshots.Where(snapshot => snapshot.Day >= from && snapshot.Day <= to))
            .Where(position => position.WalletAddress == walletAddress && !position.ClosedAt.HasValue);
    }
}