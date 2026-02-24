using CryptoWatcher.Modules.Aave.Application.Abstractions;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Aave.Application.Services;

public class AaveAccountStatusService
{
    private readonly IAavePositionRepository _repository;
    private readonly IAaveHealthFactorCalculator _aaveHealthFactorCalculator;

    public AaveAccountStatusService(IAavePositionRepository repository,
        IAaveHealthFactorCalculator aaveHealthFactorCalculator)
    {
        _repository = repository;
        _aaveHealthFactorCalculator = aaveHealthFactorCalculator;
    }

    public async Task<double> CalculateHealthFactorAsync(IReadOnlyCollection<EvmAddress> wallets, DateOnly day,
        CancellationToken ct = default)
    {
        var positions = await _repository.GetActiveForWalletAsync(wallets, day, ct);

        var hf = _aaveHealthFactorCalculator.CalculateHealthFactor(positions);

        return hf;
    }
}