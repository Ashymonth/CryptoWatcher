using System.Numerics;

namespace CryptoWatcher.Extensions;

public static class BigIntegerExtensions
{
    public static decimal ToDecimal(this BigInteger bigInteger, int decimals)
    {
        return (decimal)bigInteger / (decimal)BigInteger.Pow(10, decimals);
    }
}