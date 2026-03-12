using CryptoWatcher.Infrastructure.Configs;
using Serilog;
using Serilog.Sinks.OpenTelemetry;

namespace CryptoWatcher.Host.Extensions;

public static class LoggingExtensions
{
    public static void AddConfiguredSerilog(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
        builder.Host.UseSerilog((context, _, configuration) =>
        {
            var otelEndpoint = context.Configuration
                .GetSection(nameof(ExternalServicesConfig))
                .GetValue<string>(nameof(ExternalServicesConfig.Otel));

            configuration
                .ReadFrom.Configuration(builder.Configuration)
                .WriteTo.OpenTelemetry(options =>
                {
                    options.Endpoint = otelEndpoint;
                    options.Protocol = OtlpProtocol.Grpc;
                    options.ResourceAttributes = new Dictionary<string, object>
                    {
                        ["service.name"] = "crypto_watcher"
                    };
                }, true);
        });
    }
}