using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace CryptoWatcher.Modules.Aave.Infrastructure.Client.UiPoolDataProvider.Contracts.UserReserve;

public class UserReserveData
{
    [Parameter("address", "underlyingAsset", 1)]
    public string UnderlyingAsset { get; set; } = null!;

    [Parameter("uint256", "scaledATokenBalance", 2)]
    public BigInteger ScaledATokenBalance { get; set; }

    [Parameter("bool", "usageAsCollateralEnabledOnUser", 3)]
    public bool UsageAsCollateralEnabled { get; set; }

    [Parameter("uint256", "scaledVariableDebt", 4)]
    public BigInteger ScaledVariableDebt { get; set; }
}