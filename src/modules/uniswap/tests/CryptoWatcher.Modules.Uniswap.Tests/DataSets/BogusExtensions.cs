using Bogus;

namespace CryptoWatcher.Modules.Uniswap.Tests.DataSets;

public static class BogusExtensions
{
    private static readonly ThreadLocal<Crypto> _crypto = new(() => new Crypto());

    public static Crypto Crypto(this Faker faker) => _crypto.Value!;
}