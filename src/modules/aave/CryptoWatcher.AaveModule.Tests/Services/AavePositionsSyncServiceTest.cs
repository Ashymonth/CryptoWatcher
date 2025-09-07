using AutoFixture;
using CryptoWatcher.AaveModule.Abstractions;
using CryptoWatcher.AaveModule.Entities;
using CryptoWatcher.AaveModule.Models;
using CryptoWatcher.AaveModule.Services;
using CryptoWatcher.AaveModule.Specifications;
using CryptoWatcher.Abstractions;
using CryptoWatcher.Shared.Entities;
using CryptoWatcher.Shared.ValueObjects;
using JetBrains.Annotations;
using Moq;

namespace CryptoWatcher.AaveModule.Tests.Services;

[TestSubject(typeof(AavePositionsSyncService))]
public class AavePositionsSyncServiceTest
{
    private static readonly AaveNetwork TestNetwork = AaveNetwork.CeloNetwork;
    private static readonly Wallet TestWallet = new() { Address = Guid.CreateVersion7().ToString() };

    private readonly Mock<IAaveProvider> _aaveProviderMock = new();
    private readonly Mock<IAaveTokenEnricher> _tokenEnricherMock = new();
    private readonly Mock<IRepository<AavePosition>> _aavePositionRepositoryMock = new();

    public AavePositionsSyncServiceTest()
    {
        _aavePositionRepositoryMock.Setup(repository => repository.UnitOfWork)
            .Returns(new Mock<IUnitOfWork>().Object);
    }

    [Fact]
    public async Task SyncPositionsAsyncTest_WhenAllPositionsEmpty_ShouldReturnEmptyList()
    {
        var fixture = new Fixture();

        var randomSyncDay = DateOnly.FromDateTime(fixture.Create<DateTime>());

        _aaveProviderMock.Setup(provider =>
                provider.GetLendingPositionAsync(TestNetwork, TestWallet, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fixture.CreateMany<EmptyAaveLendingPosition>().Cast<AaveLendingPosition>().ToList());

        SetupEmptyListFromRepo();

        var service = CreateService();

        var result = await service.SyncPositionsAsync(AaveNetwork.CeloNetwork, TestWallet, randomSyncDay,
            TestContext.Current.CancellationToken);

        Assert.Empty(result);
    }

    [Theory]
    [InlineData(AavePositionType.Borrowed)]
    [InlineData(AavePositionType.Supplied)]
    public async Task SyncPositionsAsyncTest_WhenOnlyBorrowedOrSuppliedPositions_ShouldReturnAllPositions(
        AavePositionType expectedPositionType)
    {
        var fixture = new Fixture();

        var expectedPositions = expectedPositionType switch
        {
            AavePositionType.Borrowed => fixture.CreateMany<BorrowedAaveLendingPosition>().Cast<AaveLendingPosition>().ToList(),
            AavePositionType.Supplied => fixture.CreateMany<SuppliedAaveLendingPosition>().Cast<AaveLendingPosition>().ToList(),
            _ => throw new ArgumentOutOfRangeException(nameof(expectedPositionType), expectedPositionType, null)
        };

        var syncDay = DateOnly.FromDateTime(fixture.Create<DateTime>());

        _aaveProviderMock.Setup(provider =>
                provider.GetLendingPositionAsync(TestNetwork, TestWallet, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedPositions);

        var expectedSnapshotTokens = new List<TokenInfo>();
        foreach (var expectedPosition in expectedPositions)
        {
            var expectedTokenInfo = fixture.Create<TokenInfo>();
            _tokenEnricherMock.Setup(enricher => enricher.GetEnrichedTokenInfoAsync(TestNetwork,
                    (CalculatableAaveLendingPosition)expectedPosition, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTokenInfo);
            expectedSnapshotTokens.Add(expectedTokenInfo);
        }

        SetupEmptyListFromRepo();

        var service = CreateService();

        var actual = await service.SyncPositionsAsync(AaveNetwork.CeloNetwork, TestWallet, syncDay,
            TestContext.Current.CancellationToken);

        Assert.Equal(expectedPositions.Count, actual.Count);

        for (var index = 0; index < actual.Count; index++)
        {
            var actualPosition = actual[index];
            Assert.Equal(syncDay, actualPosition.CreatedAtDay);
            Assert.Equal(TestNetwork.Name, actualPosition.Network);
            Assert.Equal(expectedPositionType, actualPosition.PositionType);
            Assert.Null(actualPosition.ClosedAtDay);

            Assert.Single(actualPosition.PositionSnapshots);

            var expectedSnapshot = expectedSnapshotTokens[index];
            var actualSnapshot = actualPosition.PositionSnapshots.First();

            Assert.Equal(syncDay, actualSnapshot.Day);
            Assert.Equal(actualPosition.Id, actualSnapshot.PositionId);
            Assert.Equivalent(expectedSnapshot, actualSnapshot.Token);
        }
    }

    private AavePositionsSyncService CreateService()
    {
        return new AavePositionsSyncService(_aaveProviderMock.Object, _tokenEnricherMock.Object,
            _aavePositionRepositoryMock.Object);
    }

    private void SetupEmptyListFromRepo()
    {
        _aavePositionRepositoryMock.Setup(repository =>
                repository.ListAsync(It.IsAny<AavePositionsWithSnapshotsSpecification>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);
    }
}