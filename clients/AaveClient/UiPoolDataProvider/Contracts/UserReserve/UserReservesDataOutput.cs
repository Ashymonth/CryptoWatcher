using Nethereum.ABI.FunctionEncoding.Attributes;

namespace AaveClient.UiPoolDataProvider.Contracts;

[FunctionOutput]
public class UserReservesResponse
{
    [Parameter("tuple[]", "", 1)] public List<UserReserveData> ReservesData { get; set; } = null!;

    [Parameter("uint8", "", 2)]
    public byte ReservesCount { get; set; }
}