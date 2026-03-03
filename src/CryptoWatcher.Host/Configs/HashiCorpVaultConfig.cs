namespace CryptoWatcher.Host.Configs;

public class HashiCorpVaultConfig
{
    public Uri Address { get; set; } = null!;

    public string Token { get; set; } = null!;

    public string[] Path { get; set; } = [];

    public string Prefix { get; set; } = null!;

    public string Mount { get; set; } = null!;
}