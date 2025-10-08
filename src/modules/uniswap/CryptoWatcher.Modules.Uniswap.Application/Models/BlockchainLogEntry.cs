namespace CryptoWatcher.Modules.Uniswap.Application.Models;

public record BlockchainLogEntry
{
    public required string TransactionHash { get; init; } = null!;

    public required string Data { get; init; } = null!;

    public required string Address { get; init; } = null!;
}