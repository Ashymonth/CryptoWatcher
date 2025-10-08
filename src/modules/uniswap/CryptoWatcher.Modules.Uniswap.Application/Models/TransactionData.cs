using CryptoWatcher.Shared.ValueObjects;

namespace CryptoWatcher.Modules.Uniswap.Application.Models;

public record TransactionData
{
    public required string WalletAddress { get; init; } = null!;

    public required TokenPair TokenPair { get; init; } = null!;
}