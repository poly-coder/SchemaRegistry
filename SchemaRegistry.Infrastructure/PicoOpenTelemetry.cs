using Microsoft.Extensions.DependencyInjection;

namespace Pico.OpenTelemetry;

public static class PicoOpenTelemetryExtensions
{
    public static IServiceCollection AddOpenTelemetrySources(
        this IServiceCollection services,
        params string[] sourceNames
    )
    {
        services
            .AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                foreach (var name in sourceNames)
                {
                    metrics.AddMeter(name);
                }
            })
            .WithTracing(tracing =>
            {
                foreach (var name in sourceNames)
                {
                    tracing.AddSource(name);
                }
            });

        return services;
    }

    public static IServiceCollection AddOpenTelemetryMetricsSources(
        this IServiceCollection services,
        params string[] sourceNames
    )
    {
        services
            .AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                foreach (var name in sourceNames)
                {
                    metrics.AddMeter(name);
                }
            });

        return services;
    }

    public static IServiceCollection AddOpenTelemetryTracingSources(
        this IServiceCollection services,
        params string[] sourceNames
    )
    {
        services
            .AddOpenTelemetry()
            .WithTracing(tracing =>
            {
                foreach (var name in sourceNames)
                {
                    tracing.AddSource(name);
                }
            });

        return services;
    }
}
