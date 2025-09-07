using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace AaveClient.UiPoolDataProvider.Contracts.ReservesData;

[FunctionOutput]
public class AggregatedReserveData
{
    [Parameter("address", "underlyingAsset", 1)]
    public string UnderlyingAsset { get; set; } = null!;

    [Parameter("string", "name", 2)]
    public string Name { get; set; } = null!;

    [Parameter("string", "symbol", 3)]
    public string Symbol { get; set; } = null!;

    [Parameter("uint256", "decimals", 4)]
    public BigInteger Decimals { get; set; }

    [Parameter("uint256", "baseLTVasCollateral", 5)]
    public BigInteger BaseLTVasCollateral { get; set; }

    [Parameter("uint256", "reserveLiquidationThreshold", 6)]
    public BigInteger ReserveLiquidationThreshold { get; set; }

    [Parameter("uint256", "reserveLiquidationBonus", 7)]
    public BigInteger ReserveLiquidationBonus { get; set; }

    [Parameter("uint256", "reserveFactor", 8)]
    public BigInteger ReserveFactor { get; set; }

    [Parameter("bool", "usageAsCollateralEnabled", 9)]
    public bool UsageAsCollateralEnabled { get; set; }

    [Parameter("bool", "borrowingEnabled", 10)]
    public bool BorrowingEnabled { get; set; }

    [Parameter("bool", "isActive", 11)]
    public bool IsActive { get; set; }

    [Parameter("bool", "isFrozen", 12)]
    public bool IsFrozen { get; set; }

    [Parameter("uint128", "liquidityIndex", 13)]
    public BigInteger LiquidityIndex { get; set; }

    [Parameter("uint128", "variableBorrowIndex", 14)]
    public BigInteger VariableBorrowIndex { get; set; }

    [Parameter("uint128", "liquidityRate", 15)]
    public BigInteger LiquidityRate { get; set; }

    [Parameter("uint128", "variableBorrowRate", 16)]
    public BigInteger VariableBorrowRate { get; set; }

    [Parameter("uint40", "lastUpdateTimestamp", 17)]
    public long LastUpdateTimestamp { get; set; }

    [Parameter("address", "aTokenAddress", 18)]
    public string ATokenAddress { get; set; } = null!;

    [Parameter("address", "variableDebtTokenAddress", 19)]
    public string VariableDebtTokenAddress { get; set; } = null!;

    [Parameter("address", "interestRateStrategyAddress", 20)]
    public string InterestRateStrategyAddress { get; set; } = null!;

    [Parameter("uint256", "availableLiquidity", 21)]
    public BigInteger AvailableLiquidity { get; set; }

    [Parameter("uint256", "totalScaledVariableDebt", 22)]
    public BigInteger TotalScaledVariableDebt { get; set; }

    [Parameter("uint256", "priceInMarketReferenceCurrency", 23)]
    public BigInteger PriceInMarketReferenceCurrency { get; set; }

    [Parameter("address", "priceOracle", 24)]
    public string PriceOracle { get; set; } = null!;

    [Parameter("uint256", "variableRateSlope1", 25)]
    public BigInteger VariableRateSlope1 { get; set; }

    [Parameter("uint256", "variableRateSlope2", 26)]
    public BigInteger VariableRateSlope2 { get; set; }

    [Parameter("uint256", "baseVariableBorrowRate", 27)]
    public BigInteger BaseVariableBorrowRate { get; set; }

    [Parameter("uint256", "optimalUsageRatio", 28)]
    public BigInteger OptimalUsageRatio { get; set; }

    [Parameter("bool", "isPaused", 29)]
    public bool IsPaused { get; set; }

    [Parameter("bool", "isSiloedBorrowing", 30)]
    public bool IsSiloedBorrowing { get; set; }

    [Parameter("uint128", "accruedToTreasury", 31)]
    public BigInteger AccruedToTreasury { get; set; }

    [Parameter("uint128", "unbacked", 32)]
    public BigInteger Unbacked { get; set; }

    [Parameter("uint128", "isolationModeTotalDebt", 33)]
    public BigInteger IsolationModeTotalDebt { get; set; }

    [Parameter("bool", "flashLoanEnabled", 34)]
    public bool FlashLoanEnabled { get; set; }

    [Parameter("uint256", "debtCeiling", 35)]
    public BigInteger DebtCeiling { get; set; }

    [Parameter("uint256", "debtCeilingDecimals", 36)]
    public BigInteger DebtCeilingDecimals { get; set; }

    [Parameter("uint256", "borrowCap", 37)]
    public BigInteger BorrowCap { get; set; }

    [Parameter("uint256", "supplyCap", 38)]
    public BigInteger SupplyCap { get; set; }

    [Parameter("bool", "borrowableInIsolation", 39)]
    public bool BorrowableInIsolation { get; set; }

    [Parameter("bool", "virtualAccActive", 40)]
    public bool VirtualAccActive { get; set; }

    [Parameter("uint128", "virtualUnderlyingBalance", 41)]
    public BigInteger VirtualUnderlyingBalance { get; set; }

    [Parameter("uint128", "deficit", 42)]
    public BigInteger Deficit { get; set; }
}