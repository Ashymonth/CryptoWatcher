using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CryptoWatcher.Infrastructure.Configuration.Converters;

[UsedImplicitly]
public class UriConverter()
    : ValueConverter<Uri, string>(uri => uri.ToString(), s => new Uri(s));
