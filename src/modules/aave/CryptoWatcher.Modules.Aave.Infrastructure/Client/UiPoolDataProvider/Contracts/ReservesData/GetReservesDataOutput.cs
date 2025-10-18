using Nethereum.ABI.FunctionEncoding.Attributes;

namespace CryptoWatcher.Modules.Aave.Infrastructure.Client.UiPoolDataProvider.Contracts.ReservesData;

[FunctionOutput]
public class GetReservesDataOutput
{
    [Parameter("tuple[]", "reservesData", 1)]
    public List<AggregatedReserveData> ReservesData { get; set; } = [];

    [Parameter("tuple", "baseCurrencyInfo", 2)]
    public BaseCurrencyInfo BaseCurrencyInfo { get; set; } = null!;
}