namespace CryptoWatcher.Modules.Uniswap.Application.Models;

public record BlockchainLogBatch
{
    public required string ChainName { get; init; } = null!;

    public required IReadOnlyCollection<BlockchainLogEntry> Logs { get; init; } = [];
}