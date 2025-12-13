using CryptoWatcher.ValueObjects;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CryptoWatcher.Infrastructure.Configuration.Converters;

[UsedImplicitly]
public class EvmAddressConverter()
    : ValueConverter<EvmAddress, string>(address => address.Value, s => EvmAddress.Create(s));