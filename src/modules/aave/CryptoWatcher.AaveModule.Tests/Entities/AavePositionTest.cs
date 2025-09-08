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
    private static readonly DateOnly TestDate = DateOnly.FromDateTime(DateTime.Now);
    private readonly Fixture _fixture;

    public AavePositionTest()
    {
        _fixture = new Fixture();
        _fixture.Customize(new PositiveBigIntegerCustomization());
    }

    [Theory]
    [InlineData(AavePositionType.Supplied)]
    [InlineData(AavePositionType.Borrowed)]
    public void AddOrUpdateSnapshotTest_WhenPositionClosed_ShouldThrowException(AavePositionType positionType)
    {
        var position = CreatePosition(positionType);
        position.ClosePosition(DateOnly.MaxValue);

        Assert.Throws<InvalidOperationException>(() =>
            position.AddOrUpdateSnapshot(_fixture.Create<TokenInfo>(), 1, TestDate));
    }

    [Theory]
    [InlineData(AavePositionType.Borrowed)]
    [InlineData(AavePositionType.Supplied)]
    public void AddOrUpdateSnapshotTest_WhenSnapshotForDayExist_ShouldUpdateSnapshot(AavePositionType type)
    {
        var position = CreatePosition(type);
        var token = _fixture.Create<TokenInfo>();

        var existedTokenAmount = _fixture.Create<decimal>();
        position.AddOrUpdateSnapshot(token, existedTokenAmount, TestDate);

        var expectedTokenAmount = _fixture.Create<decimal>();
        var expectedToken = token with
        {
            PriceInUsd = _fixture.Create<decimal>(),
            Amount = _fixture.Create<decimal>()
        };

        position.AddOrUpdateSnapshot(expectedToken, expectedTokenAmount, TestDate);

        var actualSnapshot = position.PositionSnapshots.First();

        Assert.Single(position.PositionSnapshots);
        Assert.Equal(position.PreviousScaledAmount, expectedTokenAmount);
        Assert.Equivalent(new AavePositionSnapshot(position.Id, TestDate, expectedToken), actualSnapshot);
    }

    [Theory]
    [InlineData(AavePositionType.Borrowed)]
    [InlineData(AavePositionType.Supplied)]
    public void AddOrUpdateSnapshotTest_WhenSnapshotForDayNotExist_ShouldUpdateSnapshot(AavePositionType type)
    {
        var syncDate = DateOnly.FromDateTime(DateTime.Now);
        var position = CreatePosition(type);
        var token = _fixture.Create<TokenInfo>();

        position.AddOrUpdateSnapshot(token, 1, syncDate);

        var actualSnapshot = position.PositionSnapshots.First();
        var expectedSnapshot = new AavePositionSnapshot(position.Id, syncDate, token);

        Assert.Single(position.PositionSnapshots);
        Assert.Equivalent(expectedSnapshot, actualSnapshot);
    }

    [Theory]
    [InlineData(AavePositionType.Borrowed)]
    [InlineData(AavePositionType.Supplied)]
    public void AddOrUpdateSnapshotTest_WhenScaleNotChange_ShouldAddDepositEvent(AavePositionType type)
    {
        var syncDate = DateOnly.FromDateTime(DateTime.Now);
        var expectedScaledAmount = _fixture.Create<decimal>();
        var position = CreatePosition(type);
        var token = _fixture.Create<TokenInfo>();

        position.AddOrUpdateSnapshot(token, expectedScaledAmount, syncDate);
        position.AddOrUpdateSnapshot(_fixture.Create<TokenInfo>(), expectedScaledAmount, syncDate.AddDays(1));

        Assert.Single(position.PositionEvents);
        AssertThatAaveEventCorrect(
            position.PositionEvents.First(),
            position.Id,
            expectedScaledAmount,
            AavePositionEventType.Deposit);
    }

    [Theory]
    [InlineData(100, 150, AavePositionEventType.Deposit)]
    [InlineData(100, 50, AavePositionEventType.Withdrawal)]
    public void AddOrUpdateSnapshotTest_WhenScaleChange_ShouldUpdateSnapshot(
        decimal oldScaleAmount,
        decimal newScaleAmount,
        AavePositionEventType eventType)
    {
        var position = CreatePosition(AavePositionType.Borrowed);
        var token = _fixture.Create<TokenInfo>();

        position.AddOrUpdateSnapshot(token, oldScaleAmount, TestDate);
        position.AddOrUpdateSnapshot(token, newScaleAmount, TestDate);

        var expectedAmount = eventType == AavePositionEventType.Deposit
            ? newScaleAmount - oldScaleAmount
            : oldScaleAmount - newScaleAmount;

        Assert.Equal(2, position.PositionEvents.Count);
        AssertThatAaveEventCorrect(
            position.PositionEvents.Last(),
            position.Id,
            expectedAmount,
            eventType);
    }

    private AavePosition CreatePosition(AavePositionType type)
    {
        return new AavePosition(
            AaveNetwork.CeloNetwork,
            TestWallet,
            type,
            _fixture.Create<string>(),
            TestDate);
    }

    private static void AssertThatAaveEventCorrect(
        AavePositionEvent @event,
        Guid positionId,
        decimal amount,
        AavePositionEventType type)
    {
        Assert.Equal(positionId, @event.PositionId);
        Assert.Equal(TestDate, @event.Date);
        Assert.Equal(amount, @event.Amount);
        Assert.Equal(type, @event.EventType);
    }
}