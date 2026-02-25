using CryptoWatcher.Abstractions.Reports;
using CryptoWatcher.Extensions;
using CryptoWatcher.Models;
using CryptoWatcher.Modules.Aave.Application.Abstractions;
using CryptoWatcher.Modules.Aave.Entities;
using CryptoWatcher.Modules.Aave.Models;
using CryptoWatcher.Shared.Entities;
using CryptoWatcher.ValueObjects;

namespace CryptoWatcher.Modules.Aave.Application.Services;

public class AaveReportDataService : IPlatformDailyReportDataProvider
{
    private readonly IAaveReportQuery _reportQuery;

    public AaveReportDataService(IAaveReportQuery reportQuery)
    {
        _reportQuery = reportQuery;
    }

    public async Task<PlatformDailyReportData> GetReportDataAsync(IReadOnlyCollection<Wallet> wallets,
        DateOnly from,
        DateOnly to, CancellationToken ct = default)
    {
        var addresses = wallets.Select(wallet => wallet.Address).ToArray();
        var positions =
            await _reportQuery.GetPositionsForReportAsync(addresses, from, to, ct);

        var result = new Dictionary<EvmAddress, List<PlatformDailyReport>>();
        foreach (var positionsByWallet in positions.GroupBy(static position => position.WalletAddress))
        {
            foreach (var position in positionsByWallet)
            {
                var sign = position.PositionType == AavePositionType.Borrowed ? -1 : 1;
                var reportItems = position.Snapshots.OrderBy(static snapshot => snapshot.Day)
                    .Select(snapshot =>
                    {
                        var previousDay = snapshot.Day.AddDays(-1);
                        var profitInUsd = position.CalculateProfitInUsd(previousDay, snapshot.Day);
                        var profitInToken = position.CalculateProfitInToken(previousDay, snapshot.Day);

                        return new AaveDailyReportItem
                        {
                            Day = snapshot.Day,
                            NetworkName = position.Network,
                            TokenSymbol = position.Token0.Symbol,
                            PositionInUsd = snapshot.Token0.AmountInUsd * sign,
                            PositionGrowInUsd = profitInUsd.Amount * sign,

                            PositionInToken = snapshot.Token0.Amount * sign,
                            DailyProfitInUsd = profitInToken.Amount * snapshot.Token0.PriceInUsd * sign,
                            DailyProfitInUsdPercent = profitInUsd.Percent * sign,
                            DailyProfitInToken = profitInToken.Amount * sign,
                            CashFlowsInUsd =
                                position.CashFlows.CalculateNetTokenCashFlowInUsd(snapshot.Day, snapshot.Day),
                            CashFlowsInToken =
                                position.CashFlows.CalculateNetTokenCashFlowInToken(snapshot.Day, snapshot.Day),
                        };
                    })
                    .ToArray();

                var lastTokenPrice = position.Snapshots.LastOrDefault()?.Token0.PriceInUsd ?? 0;
                var profitInUsd = position.CalculateProfitInUsd(from, to);

                var profitInToken = position.CalculateProfitInToken(from, to);
                var dailyReport = new AaveDailyReport
                {
                    PositionInUsd = reportItems.LastOrDefault()?.PositionInUsd ?? 0 * sign,
                    PositionInToken = reportItems.LastOrDefault()?.PositionInToken ?? 0 * sign,
                    ProfitInUsd = profitInToken.Amount * lastTokenPrice * sign,
                    ProfitInPercent = profitInUsd.Percent * sign,
                    PositionGrowInUsd = profitInUsd.Amount * sign,
                    ProfitInToken = profitInToken.Amount * sign,
                    ReportItems = reportItems
                };

                if (!result.TryGetValue(position.WalletAddress, out var dailyReports))
                {
                    dailyReports = [];
                    result.Add(position.WalletAddress, dailyReports);
                }

                dailyReports.Add(dailyReport);
            }
        }

        return new PlatformDailyReportData
        {
            PlatformName = "Aave",
            Reports = result
        };
    }
}