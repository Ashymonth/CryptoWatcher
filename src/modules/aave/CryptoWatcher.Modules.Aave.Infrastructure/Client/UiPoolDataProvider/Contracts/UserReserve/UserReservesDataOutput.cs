using Nethereum.ABI.FunctionEncoding.Attributes;

namespace CryptoWatcher.Modules.Aave.Infrastructure.Client.UiPoolDataProvider.Contracts.UserReserve;

[FunctionOutput]
public class UserReservesResponse
{
    [Parameter("tuple[]", "", 1)] public List<UserReserveData> ReservesData { get; set; } = null!;

    [Parameter("uint8", "", 2)]
    public byte ReservesCount { get; set; }
}