using CryptoWatcher.AaveModule.Entities;
using CryptoWatcher.AaveModule.Models;
using CryptoWatcher.AaveModule.Specifications;
using CryptoWatcher.Abstractions;
using CryptoWatcher.Shared.Entities;

namespace CryptoWatcher.AaveModule.Services;

public interface IAaveReportService
{
    Task<List<AavePositionReport>> CreateReport(Wallet wallet, DateOnly from, DateOnly to,
        CancellationToken ct = default);
}

internal class AaveReportService : IAaveReportService
{
    private readonly IRepository<AavePosition> _repository;

    public AaveReportService(IRepository<AavePosition> repository)
    {
        _repository = repository;
    }

    public async Task<List<AavePositionReport>> CreateReport(Wallet wallet, DateOnly from, DateOnly to,
        CancellationToken ct = default)
    {
        var positions =
            await _repository.ListAsync(new AavePositionsWithSnapshotsAndEventsSpecification(wallet, from, to), ct);

        var result = new List<AavePositionReport>();
        foreach (var position in positions)
        {
            var positionReport = new AavePositionReport
            {
                ReportItems = position.PositionSnapshots.OrderBy(snapshot => snapshot.Day)
                    .Select(snapshot =>
                    {
                        return new AavePositionReportItem
                        {
                            Day = snapshot.Day,
                            Position = snapshot.Token,
                            PositionChange = position.CalculateAbsoluteProfitInUsd(snapshot.Day.AddDays(-1), snapshot.Day),
                            CommissionInToken =
                                position.CalculateAbsoluteProfitInToken(snapshot.Day.AddDays(-1), snapshot.Day)
                        };
                    })
                    .ToArray()
            };

            result.Add(positionReport);
        }

        return result;
    }
}