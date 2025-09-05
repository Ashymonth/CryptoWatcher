using CryptoWatcher.AaveModule.Entities;
using CryptoWatcher.Shared.ValueObjects;

namespace CryptoWatcher.AaveModule.Models;

public class AaveLendingPosition
{
    public required AaveNetwork Network { get; set; } = null!;

    public required AavePositionType PositionType { get; init; }

    public required TokenInfoWithAddress Token { get; init; } = null!;
}