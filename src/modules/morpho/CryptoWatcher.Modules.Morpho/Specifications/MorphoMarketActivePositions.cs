using Ardalis.Specification;
using CryptoWatcher.Modules.Morpho.Entities;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Morpho.Specifications;

public sealed class MorphoMarketActivePositions : Specification<MorphoMarketPosition>
{
    public MorphoMarketActivePositions(EvmAddress walletAddress)
    {
        Query.Where(position => position.WalletAddress.Value == walletAddress.Value && !position.ClosedAt.HasValue);
    }
}