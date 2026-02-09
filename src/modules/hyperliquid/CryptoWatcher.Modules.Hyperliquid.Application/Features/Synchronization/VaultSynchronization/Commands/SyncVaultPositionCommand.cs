using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization.Commands;

public record SyncVaultPositionCommand(EvmAddress Wallet, DateTime SnapshotDate);