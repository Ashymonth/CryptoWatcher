using CryptoWatcher.ValueObjects;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CryptoWatcher.Modules.Infrastructure.Shared.Persistence.Converters;

[UsedImplicitly]
public class TransactionHashConverter()
    : ValueConverter<TransactionHash, string>(address => address.Value, s => TransactionHash.FromString(s));