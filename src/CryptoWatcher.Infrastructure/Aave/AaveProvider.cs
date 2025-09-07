using AaveClient;
using CryptoWatcher.AaveModule.Abstractions;
using CryptoWatcher.AaveModule.Entities;
using CryptoWatcher.AaveModule.Models;
using CryptoWatcher.Extensions;
using CryptoWatcher.Shared.Entities;

namespace CryptoWatcher.Infrastructure.Aave;

internal class AaveProvider : IAaveProvider
{
    private readonly IAaveApiClient _aaveApiClient;
    private readonly IAaveMainnetProvider _aaveMainnetProvider;

    public AaveProvider(IAaveApiClient aaveApiClient, IAaveMainnetProvider aaveMainnetProvider)
    {
        _aaveApiClient = aaveApiClient;
        _aaveMainnetProvider = aaveMainnetProvider;
    }

    public async Task<List<AaveLendingPosition>> GetLendingPositionAsync(AaveNetwork aaveNetwork, Wallet wallet,
        CancellationToken ct = default)
    {
        var mainnet = _aaveMainnetProvider.GetMainnetAddressByNetworkName(aaveNetwork);

        var networkInfo = GetNetworkInfo(aaveNetwork);

        var userReserves =
            await _aaveApiClient.UiPoolDataProviderFetcher.GetUserReservesDataAsync(mainnet, networkInfo,
                wallet.Address);

        var reserveDataDictionary =
            (await _aaveApiClient.UiPoolDataProviderFetcher.GetReservesDataAsync(mainnet, networkInfo)).ReservesData
            .ToDictionary(data => data.UnderlyingAsset);

        var result = new List<AaveLendingPosition>();

        foreach (var userReserveData in userReserves)
        {
            if (userReserveData.ScaledATokenBalance == 0 && userReserveData.ScaledVariableDebt == 0)
            {
                result.Add(AaveLendingPosition.CreateEmpty(aaveNetwork, userReserveData.UnderlyingAsset));

                continue;
            }

            if (!reserveDataDictionary.TryGetValue(userReserveData.UnderlyingAsset, out var reserveData))
            {
                throw new Exception("Can't find reserve data");
            }

            if (userReserveData.ScaledATokenBalance > 0)
            {
                var suppliedPosition = new AaveLendingPosition
                {
                    ScaleAmount = userReserveData.ScaledATokenBalance,
                    TokenAddress = userReserveData.UnderlyingAsset,
                    PositionType = AavePositionType.Supplied,
                    PoolIndex = reserveData.LiquidityIndex,
                    Network = aaveNetwork,
                    TokenPriceInUsd = reserveData.PriceInMarketReferenceCurrency.ToDecimal(8)
                };

                result.Add(suppliedPosition);
            }

            if (userReserveData.ScaledVariableDebt > 0)
            {
                var borrowedPosition = new AaveLendingPosition
                {
                    ScaleAmount = userReserveData.ScaledVariableDebt,
                    TokenAddress = userReserveData.UnderlyingAsset,
                    PositionType = AavePositionType.Borrowed,
                    PoolIndex = reserveData.VariableBorrowIndex,
                    Network = aaveNetwork,
                    TokenPriceInUsd = reserveData.PriceInMarketReferenceCurrency.ToDecimal(8)
                };

                result.Add(borrowedPosition);
            }
        }


        return result;
    }

    private AaveRegistry.SmartContractAddresses GetNetworkInfo(AaveNetwork aaveNetwork)
    {
        if (!Enum.TryParse<AaveNetworkType>(aaveNetwork.Name, out var network))
        {
            throw new ArgumentException(
                $"Network {aaveNetwork.Name} is not supported. Supported networks: {string.Join(", ", Enum.GetNames<AaveNetworkType>())}");
        }

        return AaveRegistry.NetworkToRpcUrl[network];
    }
}