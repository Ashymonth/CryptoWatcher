using System.Numerics;

namespace CryptoWatcher.Modules.Aave;

public static class AaveMath
{
    private const int RayDecimals = 27;
    private static readonly BigInteger Ray = BigInteger.Pow(10, RayDecimals);

    /// <summary>Calculates accrued raw amount: scaled * index (Ray mul).</summary>
    public static BigInteger CalculateAccruedRaw(BigInteger scaledAmount, BigInteger index) => RayMul(scaledAmount, index);

    /// <summary>Converts raw BigInteger to decimal with token decimals.</summary>
    public static decimal ToTokenDecimal(BigInteger rawAmount, byte tokenDecimals)
    {
        var divisor = BigInteger.Pow(10, tokenDecimals);
        return (decimal)rawAmount / (decimal)divisor; // Safe for Aave scales
    }

    /// <summary>Normalizes LTV from basis points to decimal (e.g., 7500 -> 0.75).</summary>
    public static decimal NormalizeLtv(BigInteger rawLtv) => decimal.Round((decimal)rawLtv / 10000m, 4, MidpointRounding.AwayFromZero);
 
    /// <summary>Multiplies two Ray-scaled values, dividing by Ray (floor division).</summary>
    private static BigInteger RayMul(BigInteger a, BigInteger b) => (a * b) / Ray;
}