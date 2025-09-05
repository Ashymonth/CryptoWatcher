using AaveClient;
using CryptoWatcher.AaveModule.Abstractions;
using CryptoWatcher.AaveModule.Entities;
using CryptoWatcher.AaveModule.Models;
using CryptoWatcher.Abstractions;
using CryptoWatcher.Infrastructure.Services;
using CryptoWatcher.Shared.Entities;
using CryptoWatcher.Shared.ValueObjects;
using AaveNetwork = CryptoWatcher.AaveModule.Models.AaveNetwork;

namespace CryptoWatcher.Infrastructure.Integrations;

public class AaveProvider : IAaveProvider
{
    private readonly IAaveApiClient _aaveApiClient;
    private readonly ITokenEnricher _tokenEnricher;

    public AaveProvider(IAaveApiClient aaveApiClient, TokenEnricher tokenEnricher)
    {
        _aaveApiClient = aaveApiClient;
        _tokenEnricher = tokenEnricher;
    }

    public async Task<List<AaveLendingPosition>> GetLendingPositionAsync(AaveNetwork aaveNetwork, Wallet wallet,
        CancellationToken ct = default)
    {
        _ = Enum.TryParse<AaveNetworkType>(aaveNetwork.Value, out var network)
            ? network
            : throw new ArgumentException(
                $"Network {aaveNetwork.Value} is not supported. Supported networks: {string.Join(", ", Enum.GetNames<AaveNetworkType>())}"
            );

        var networkInfo = NetworkRegistry.NetworkToRpcUrl[network];

        var positions =
            await _aaveApiClient.UiPoolDataProviderFetcher.GetUserReservesDataAsync(networkInfo, wallet.Address);

        var result = new List<AaveLendingPosition>();

        foreach (var userReserveData in positions)
        {
            if (userReserveData.ScaledATokenBalance == 0 && userReserveData.ScaledVariableDebt == 0)
            {
                continue;
            }
            
            var token = await _tokenEnricher.EnrichTokenAsync(networkInfo.RpcAddress,
                new Token { Balance = userReserveData.ScaledATokenBalance, Address = userReserveData.UnderlyingAsset },
                ct);

            if (userReserveData.ScaledATokenBalance > 0)
            {
                var suppliedPosition = new AaveLendingPosition
                {
                    PositionType = AavePositionType.Supplied,
                    Network = aaveNetwork,
                    Token = token
                };

                result.Add(suppliedPosition);
            }

            if (userReserveData.ScaledVariableDebt > 0)
            {
                var borrowedPosition = new AaveLendingPosition
                {
                    PositionType = AavePositionType.Borrowed,
                    Network = aaveNetwork,
                    Token = token
                };

                result.Add(borrowedPosition);
            }
        }

        return result;
    }
}