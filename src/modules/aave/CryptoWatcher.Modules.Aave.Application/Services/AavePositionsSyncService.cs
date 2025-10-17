using CryptoWatcher.Abstractions;
using CryptoWatcher.Modules.Aave.Abstractions;
using CryptoWatcher.Modules.Aave.Application.Abstractions;
using CryptoWatcher.Modules.Aave.Entities;
using CryptoWatcher.Modules.Aave.Models;
using CryptoWatcher.Modules.Aave.Specifications;
using CryptoWatcher.Shared.Entities;
using CryptoWatcher.ValueObjects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CryptoWatcher.Modules.Aave.Application.Services;

public class AavePositionsSyncService : IAavePositionsSyncService
{
    private readonly IAaveProvider _aaveProvider;
    private readonly IAaveTokenEnricher _aaveTokenEnricher;
    private readonly IRepository<AavePosition> _aavePositionRepository;
    private readonly TimeProvider _timeProvider;
    private readonly ILogger<AavePositionsSyncService> _logger;

    public AavePositionsSyncService(IAaveProvider aaveProvider, IAaveTokenEnricher aaveTokenEnricher,
        IRepository<AavePosition> aavePositionRepository, TimeProvider timeProvider,
        ILogger<AavePositionsSyncService>? logger = null)
    {
        _aaveProvider = aaveProvider;
        _aaveTokenEnricher = aaveTokenEnricher;
        _aavePositionRepository = aavePositionRepository;
        _timeProvider = timeProvider;

        _logger = logger ?? NullLogger<AavePositionsSyncService>.Instance;
    }

    public async Task<List<AavePosition>> SyncPositionsAsync(
        AaveNetwork network,
        Wallet wallet, 
        DateOnly syncDay,
        CancellationToken ct = default)
    {
        var existedPositions = await _aavePositionRepository.ListAsync(
            new AavePositionsWithSnapshotsSpecification(wallet.Address, syncDay, syncDay), ct);

        _logger.LogExistedPositionsForWalletCount(wallet.Address, existedPositions.Count);

        var result = new List<AavePosition>();

        var lendingPositions = await _aaveProvider.GetLendingPositionAsync(network, wallet, ct);

        _logger.LogFetchedPositionsForNetworkCount(network.Name, lendingPositions.Count);

        foreach (var lendingPosition in lendingPositions)
        {
            if (lendingPosition is EmptyAaveLendingPosition)
            {
                foreach (var position in existedPositions.Where(position =>
                             position.TokenAddress.Equals(lendingPosition.TokenAddress)))
                {
                    position.ClosePosition(syncDay);
                    result.Add(position);

                    _logger.LogPositionClosed(position.Id, position.TokenAddress);
                }

                continue;
            }

            var calculatableAaveLendingPosition = lendingPosition as CalculatableAaveLendingPosition ??
                                                  throw new InvalidOperationException(
                                                      "To calculate position amount, lending position must inherit from CalculatableAaveLendingPosition class");

            var tokenInfo =
                await _aaveTokenEnricher.EnrichTokenAsync(network, calculatableAaveLendingPosition, ct);

            var positionType = calculatableAaveLendingPosition.DeterminePositionType();

            var currentPosition = existedPositions.FirstOrDefault(position =>
                position.TokenAddress.Equals(lendingPosition.TokenAddress) && position.PositionType == positionType);

            if (currentPosition is null)
            {
                currentPosition =
                    new AavePosition(network, wallet, positionType, EvmAddress.Create(lendingPosition.TokenAddress),
                        syncDay);

                _aavePositionRepository.Insert(currentPosition);

                _logger.LogCreateAavePosition(currentPosition.TokenAddress, tokenInfo);
            }

            else
            {
                _aavePositionRepository.Update(currentPosition);

                _logger.LogUpdateAavePosition(currentPosition.TokenAddress, tokenInfo);
            }

            var positionScaleAmount = calculatableAaveLendingPosition.CalculatePositionScaleInToken();
            currentPosition.AddOrUpdateSnapshot(tokenInfo, positionScaleAmount, syncDay, _timeProvider);

            result.Add(currentPosition);
        }

        await _aavePositionRepository.UnitOfWork.SaveChangesAsync(ct);

        return result;
    }
}