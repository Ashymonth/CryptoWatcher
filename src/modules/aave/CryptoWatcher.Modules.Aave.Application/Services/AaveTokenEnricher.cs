using CryptoWatcher.Abstractions;
using CryptoWatcher.Modules.Aave.Abstractions;
using CryptoWatcher.Modules.Aave.Application.Abstractions;
using CryptoWatcher.Modules.Aave.Models;
using CryptoWatcher.Modules.Aave.Services;
using CryptoWatcher.Shared.ValueObjects;

namespace CryptoWatcher.Modules.Aave.Application.Services;

public class AaveTokenEnricher : IAaveTokenEnricher
{
    private readonly IAaveMainnetProvider _aaveMainnetProvider;
    private readonly ITokenEnricher _tokenEnricher;

    public AaveTokenEnricher(IAaveMainnetProvider aaveMainnetProvider, ITokenEnricher tokenEnricher)
    {
        _aaveMainnetProvider = aaveMainnetProvider;
        _tokenEnricher = tokenEnricher;
    }

    public async Task<TokenInfo> EnrichTokenAsync(AaveNetwork network,
        CalculatableAaveLendingPosition position,
        CancellationToken ct = default)
    {
        var token = new Token { Address = position.TokenAddress, Balance = position.CalculateAmountWithInterest() };

        var mainnetAddress = _aaveMainnetProvider.GetMainnetAddressByNetworkName(network);

        return await _tokenEnricher.EnrichTokenAsync(mainnetAddress, token, position.TokenPriceInUsd, ct);
    }
}