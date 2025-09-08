using AutoFixture;
using CryptoWatcher.AaveModule.Entities;
using CryptoWatcher.AaveModule.Models;
using CryptoWatcher.Shared.Entities;
using CryptoWatcher.Shared.ValueObjects;
using JetBrains.Annotations;

namespace CryptoWatcher.AaveModule.Tests.Entities;

[TestSubject(typeof(AavePosition))]
public class AavePositionTest
{
    private static readonly Wallet TestWallet = new() { Address = Guid.CreateVersion7().ToString() };

    [Fact]
    public void AddOrUpdateSnapshotTest_WhenPositionClosed_ShouldThrowException()
    {
        var fixture = new Fixture();
        var position = new AavePosition(AaveNetwork.CeloNetwork, TestWallet, AavePositionType.Borrowed,
            fixture.Create<string>(), DateOnly.FromDateTime(DateTime.Now));

        position.ClosePosition(DateOnly.MaxValue);
        
        Assert.Throws<InvalidOperationException>(() =>
            position.AddOrUpdateSnapshot(fixture.Create<TokenInfo>(), 1, DateOnly.MaxValue));
    }

    [Fact]
    public void AddOrUpdateSnapshotTest_WhenSnapshotForDayExist_ShouldUpdateSnapshot()
    {
        var fixture = new Fixture();
        fixture.Customize<DateOnly>(composer => composer.FromFactory<DateTime>(DateOnly.FromDateTime));

        var syncDate = DateOnly.FromDateTime(DateTime.Now);

        var position = new AavePosition(AaveNetwork.CeloNetwork, TestWallet, AavePositionType.Borrowed,
            fixture.Create<string>(), DateOnly.FromDateTime(DateTime.Now));

        var token = fixture.Create<TokenInfo>();

        var existedSnapshot = new AavePositionSnapshot(position.Id, syncDate, token);

        var expectedSnapshot = new AavePositionSnapshot(position.Id, syncDate,
            token with { PriceInUsd = fixture.Create<decimal>(), Amount = fixture.Create<decimal>() });

        position.AddOrUpdateSnapshot(existedSnapshot.Token, 1, syncDate);
        position.AddOrUpdateSnapshot(expectedSnapshot.Token, 1, syncDate);

        var actualSnapshot = position.PositionSnapshots.First();

        Assert.Equivalent(expectedSnapshot, actualSnapshot);
    }
}