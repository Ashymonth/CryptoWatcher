namespace HyperliquidClient.UserNonFundingLedgerUpdates.Contracts;

public record VaultWithdraw(string Vault, decimal NetWithdrawnUsd, string Type) : Delta(Type);