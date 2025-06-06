using Ardalis.Specification;
using CryptoWatcher.Entities;

namespace CryptoWatcher.PoolHistorySyncFeature;

public sealed class GetPositionsByWalletAndNetworkSpecification : Specification<LiquidityPoolPosition>
{
    public GetPositionsByWalletAndNetworkSpecification(Network network, Wallet wallet)
    {
        Query.Where(position => position.Network.Name == network.Name && position.Wallet.Address == wallet.Address);
    }
}