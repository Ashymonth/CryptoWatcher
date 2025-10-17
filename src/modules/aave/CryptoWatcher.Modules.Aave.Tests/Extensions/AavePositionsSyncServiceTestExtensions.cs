using AutoFixture;
using CryptoWatcher.Abstractions;
using CryptoWatcher.Modules.Aave.Application.Abstractions;
using CryptoWatcher.Modules.Aave.Entities;
using CryptoWatcher.Modules.Aave.Models;
using CryptoWatcher.Modules.Aave.Specifications;
using CryptoWatcher.Shared.ValueObjects;
using Moq;

namespace CryptoWatcher.AaveModule.Tests.Extensions;

internal static class AavePositionsSyncServiceTestExtensions
{
    public static void SetupEmptyListFromRepo(this Mock<IRepository<AavePosition>> mock)
    {
        mock.Setup(repository => repository.ListAsync(It.IsAny<AavePositionsWithSnapshotsSpecification>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);
    }

    public static List<TokenInfo> SetupAaveTokenEnricher(this Mock<IAaveTokenEnricher> mock,
        Fixture fixture,
        AaveNetwork network,
        IEnumerable<AaveLendingPosition> expectedPositions)
    {
        var expectedSnapshotTokens = new List<TokenInfo>();
        foreach (var expectedPosition in expectedPositions)
        {
            var expectedTokenInfo = fixture.Create<TokenInfo>();
            
            mock.Setup(enricher => enricher.EnrichTokenAsync(network,
                    (CalculatableAaveLendingPosition)expectedPosition, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTokenInfo);
            
            expectedSnapshotTokens.Add(expectedTokenInfo);
        }

        return expectedSnapshotTokens;
    }
}