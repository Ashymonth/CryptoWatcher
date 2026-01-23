using System.Numerics;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Uniswap.Application.Models;

public class EtherscanTransactionQuery
{
    public EvmAddress WalletAddress { get; set; } = null!;

    public int ChainId { get; set; }

    public string ApiKey { get; set; } = null!;

    public int Page { get; set; }

    public int Offset { get; set; }

    public BigInteger StartBlock { get; set; }
}