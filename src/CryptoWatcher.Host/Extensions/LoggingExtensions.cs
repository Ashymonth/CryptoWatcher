using CryptoWatcher.Infrastructure.Configs;
using Serilog;
using Serilog.Sinks.OpenTelemetry;

namespace CryptoWatcher.Host.Extensions;

public static class LoggingExtensions
{
    public static void AddConfiguredSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((_, provider, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(builder.Configuration)
                .WriteTo.Console()
                .WriteTo.OpenTelemetry(options =>
                {
                    options.Endpoint = provider.GetRequiredService<ExternalServicesConfig>().Otel.ToString();
                    options.Protocol = OtlpProtocol.Grpc;
                    options.ResourceAttributes = new Dictionary<string, object>
                    {
                        ["service.name"] = "crypto_watcher"
                    };
                }, true);
        });
    }
}