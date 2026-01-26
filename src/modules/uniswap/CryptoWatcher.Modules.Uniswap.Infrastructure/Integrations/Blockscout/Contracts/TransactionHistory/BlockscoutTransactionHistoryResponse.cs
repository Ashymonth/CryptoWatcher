using System.Text.Json.Serialization;
using CryptoWatcher.Modules.Uniswap.Infrastructure.Integrations.Blockscout.Contracts.Shared;

namespace CryptoWatcher.Modules.Uniswap.Infrastructure.Integrations.Blockscout.Contracts.TransactionHistory;

public class BlockscoutTransactionHistoryResponse
{
    public List<BlockscoutTransactionHistoryItem> Items { get; init; } = [];

    [JsonPropertyName("next_page_params")] public BlockscoutNextPageParams? NextPageParams { get; init; }
}