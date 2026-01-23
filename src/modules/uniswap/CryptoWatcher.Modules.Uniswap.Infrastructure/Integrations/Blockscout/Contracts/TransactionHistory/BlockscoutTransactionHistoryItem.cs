using System.Text.Json.Serialization;
using CryptoWatcher.Modules.Uniswap.Infrastructure.Integrations.Blockscout.Contracts.Shared;

namespace CryptoWatcher.Modules.Uniswap.Infrastructure.Integrations.Blockscout.Contracts.TransactionHistory;

public class BlockscoutTransactionHistoryItem
{
    public BlockscoutSender From { get; set; } = null!;

    public string Hash { get; set; } = null!;

    public string? Method { get; set; }

    [JsonPropertyName("block_number")] public long BlockNumber { get; set; }
}