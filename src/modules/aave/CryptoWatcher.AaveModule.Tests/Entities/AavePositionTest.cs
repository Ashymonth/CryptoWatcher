using AutoFixture;
using CryptoWatcher.AaveModule.Entities;
using CryptoWatcher.AaveModule.Models;
using CryptoWatcher.AaveModule.Tests.Customizations;
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
        fixture.Customize(new PositiveBigIntegerCustomization());
        
        var position = new AavePosition(AaveNetwork.CeloNetwork, TestWallet, AavePositionType.Borrowed,
            fixture.Create<string>(), DateOnly.FromDateTime(DateTime.Now));

        position.ClosePosition(DateOnly.MaxValue);
        
        Assert.Throws<InvalidOperationException>(() =>
            position.AddOrUpdateSnapshot(fixture.Create<TokenInfo>(), 1, DateOnly.MaxValue));
    }

    [Theory]
    [InlineData(AavePositionType.Borrowed)]
    [InlineData(AavePositionType.Supplied)]
    public void AddOrUpdateSnapshotTest_WhenSnapshotForDayExist_ShouldUpdateSnapshot(AavePositionType type)
    {
        var fixture = new Fixture();
        
        fixture.Customize(new PositiveBigIntegerCustomization());

        var syncDate = DateOnly.FromDateTime(DateTime.Now);

        var position = new AavePosition(AaveNetwork.CeloNetwork, TestWallet, type,
            fixture.Create<string>(), DateOnly.FromDateTime(DateTime.Now));

        var token = fixture.Create<TokenInfo>();

        var existedSnapshot = new AavePositionSnapshot(position.Id, syncDate, token);
        var existedTokenAmount = fixture.Create<decimal>();

        var expectedSnapshot = new AavePositionSnapshot(position.Id, syncDate,
            token with { PriceInUsd = fixture.Create<decimal>(), Amount = fixture.Create<decimal>() });

        var expectedTokenAmount = fixture.Create<decimal>();
        
        position.AddOrUpdateSnapshot(existedSnapshot.Token, existedTokenAmount, syncDate);
        
        Assert.Equal(position.PreviousScaledAmount, existedTokenAmount);;
        
        position.AddOrUpdateSnapshot(expectedSnapshot.Token, expectedTokenAmount, syncDate);

        var actualSnapshot = position.PositionSnapshots.First();

        Assert.Single(position.PositionSnapshots);
        Assert.Equal(position.PreviousScaledAmount, expectedTokenAmount);
        Assert.Equivalent(expectedSnapshot, actualSnapshot);
    }
    
    [Theory]
    [InlineData(AavePositionType.Borrowed)]
    [InlineData(AavePositionType.Supplied)]
    public void AddOrUpdateSnapshotTest_WhenSnapshotForDayNotExist_ShouldUpdateSnapshot(AavePositionType type)
    {
        var fixture = new Fixture();
        
        fixture.Customize(new PositiveBigIntegerCustomization());

        var syncDate = DateOnly.FromDateTime(DateTime.Now);

        var position = new AavePosition(AaveNetwork.CeloNetwork, TestWallet, type,
            fixture.Create<string>(), DateOnly.FromDateTime(DateTime.Now));

        var token = fixture.Create<TokenInfo>();

        var expectedSnapshot = new AavePositionSnapshot(position.Id, syncDate, token);

        position.AddOrUpdateSnapshot(expectedSnapshot.Token, 1, syncDate);

        var actualSnapshot = position.PositionSnapshots.First();

        Assert.Single(position.PositionSnapshots);
        Assert.Equivalent(expectedSnapshot, actualSnapshot);
    }
}