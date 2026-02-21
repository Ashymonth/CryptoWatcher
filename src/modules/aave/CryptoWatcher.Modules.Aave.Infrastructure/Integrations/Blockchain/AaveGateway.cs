using CryptoWatcher.Modules.Aave.Application.Abstractions;
using CryptoWatcher.Modules.Aave.Application.Models;
using CryptoWatcher.Modules.Aave.Entities;
using CryptoWatcher.Modules.Aave.Infrastructure.Integrations.Blockchain.UiPoolDataProvider;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Aave.Infrastructure.Integrations.Blockchain;

public class AaveGateway : IAaveGateway
{
    private readonly IUiPoolDataProviderFetcher _uiPoolDataProviderFetcher;

    public AaveGateway(IUiPoolDataProviderFetcher uiPoolDataProviderFetcher)
    {
        _uiPoolDataProviderFetcher = uiPoolDataProviderFetcher;
    }

    public async Task<IReadOnlyCollection<UserReserve>> GetUserReservesDataAsync(AaveChainConfiguration chain,
        EvmAddress userAddress)
    {
        var result = await _uiPoolDataProviderFetcher.GetUserReservesDataAsync(chain, userAddress);

        return result.ReservesData.Select(data => new UserReserve
        {
            UnderlyingAsset = EvmAddress.Create(data.UnderlyingAsset),
            ScaledATokenBalance = data.ScaledATokenBalance,
            ScaledVariableDebt = data.ScaledVariableDebt,
            IsCollateral = data.UsageAsCollateralEnabled
        }).ToArray();
    }

    public async Task<MarketReserveOutput> GetMarketReservesDataAsync(AaveChainConfiguration chain)
    {
        var result = await _uiPoolDataProviderFetcher.GetMarketReservesDataAsync(chain);

        return new MarketReserveOutput
        {
            NetworkBaseTokenPriceDecimals = (byte)result.BaseCurrencyInfo.NetworkBaseTokenPriceDecimals,
            AggregatedMarketReserveData = result.ReservesData.Select(data => new AggregatedMarketReserveData
            {
                UnderlyingAsset = EvmAddress.Create(data.UnderlyingAsset),
                Decimals = data.Decimals,
                LiquidityIndex = data.LiquidityIndex,
                LiquidationLtv = data.ReserveLiquidationThreshold,
                PriceInMarketReferenceCurrency = data.PriceInMarketReferenceCurrency,
                VariableBorrowIndex = data.VariableBorrowIndex,
                ReserveLiquidationThreshold = (ushort)data.ReserveLiquidationThreshold
            }).ToArray(),
        };
    }
}