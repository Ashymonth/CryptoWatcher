using CryptoWatcher.Entities;
using Nethereum.Web3;

namespace CryptoWatcher.Models;

/// <summary>
/// Context object containing network-specific services and dependencies,
/// used when interacting with an Uniswap V3 liquidity pool.
/// </summary>
/// <param name="Web3">Web3 instance used for blockchain interactions.</param>
/// <param name="Network">Network-specific constants such as addresses of core contracts (e.g., factory, tokens).</param>
public record UniswapPoolContext(IWeb3 Web3, Network Network);
