namespace HyperliquidClient.UserVaultEquities.Contracts;

public record UserVaultEquity(string VaultAddress, string Equity, long LockedUntilTimestamp);