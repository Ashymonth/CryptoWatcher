using System.Globalization;
using CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization.Abstractions;
using CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization.Models;
using CryptoWatcher.Modules.Hyperliquid.Application.Features.Synchronization.VaultSynchronization.Models.PositionUpdates;
using CryptoWatcher.Modules.Hyperliquid.Infrastructure.Integrations.Hyperliquid.Api;
using CryptoWatcher.Modules.Hyperliquid.Infrastructure.Integrations.Hyperliquid.Contracts.UserNonFundingLedgerUpdates;
using CryptoWatcher.Modules.Hyperliquid.Infrastructure.Integrations.Hyperliquid.Contracts.UserVaultEquities;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Hyperliquid.Infrastructure.Integrations.Hyperliquid;

public class HyperliquidApiGateway : IHyperliquidGateway
{
    private readonly IHyperliquidApi _hyperliquidApi;

    public HyperliquidApiGateway(IHyperliquidApi hyperliquidApi)
    {
        _hyperliquidApi = hyperliquidApi;
    }

    public async Task<HyperliquidVaultUpdate[]> GetVaultUpdatesAsync(EvmAddress walletAddress,
        DateTimeOffset from,
        CancellationToken ct = default)
    {
        var result = await _hyperliquidApi.GetUserNonFundingLedgerUpdatesAsync(
            new UserNonFundingLedgerUpdatesRequest(walletAddress, from.ToUnixTimeMilliseconds()), ct);

        return result
            .Where(update => update.Delta is VaultDeposit or VaultWithdraw)
            .Select(MapToVaultEvent)
            .OrderBy(update => update.Timestamp)
            .ToArray();
    }

    public async Task<IReadOnlyCollection<HyperliquidVault>> GetVaultsPositionsEquityAsync(EvmAddress walletAddress,
        CancellationToken ct = default)
    {
        var balance = await _hyperliquidApi.GetUserVaultEquitiesAsync(new UserVaultEquitiesRequest(walletAddress), ct);

        return balance.Select(equity => new HyperliquidVault
        {
            Balance = decimal.Parse(equity.Equity, CultureInfo.InvariantCulture),
            Address = EvmAddress.Create(equity.VaultAddress)
        }).ToArray();
    }

    private static HyperliquidVaultUpdate MapToVaultEvent(UserNonFundingLedgerUpdate update)
    {
        var timestamp = DateTimeOffset.FromUnixTimeMilliseconds(update.Time);
        return update.Delta switch
        {
            VaultDeposit vaultDeposit => new HyperliquidDepositUpdate
            {
                Amount = decimal.Parse(vaultDeposit.Usdc, CultureInfo.InvariantCulture),
                Timestamp = timestamp,
                VaultAddress = EvmAddress.Create(vaultDeposit.Vault),
                Hash = TransactionHash.FromString(update.Hash)
            },
            VaultWithdraw vaultWithdraw => new HyperliquidWithdrawUpdate
            {
                Amount = decimal.Parse(vaultWithdraw.NetWithdrawnUsd, CultureInfo.InvariantCulture),
                Timestamp = timestamp,
                VaultAddress = EvmAddress.Create(vaultWithdraw.Vault),
                Hash = TransactionHash.FromString(update.Hash)
            },
            _ => throw new ArgumentOutOfRangeException(nameof(update), update.Delta, null)
        };
    }
}