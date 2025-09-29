using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Pico.DependencyInjection;
using Pico.DependencyInjection.Testing;
using Pico.Reflection;
using Pico.Testing;

namespace SchemaRegistry.WebApi.UnitTests;

[UnitTest]
public class StartupUnitTests
{
    [Fact]
    public async Task WebApplicationDefaultsTest()
    {
        var builder = WebApplication.CreateBuilder([]);

        var capture = builder
            .Services.CaptureRegisteredServices()
            .ForTesting(new ToDisplayNameOptions(FullNames: true));

        await Verify(capture).UseSnapshotsDirectory();
    }

    [Fact]
    public async Task AddServiceDefaultsTest()
    {
        var builder = WebApplication.CreateBuilder([]);

        var capture = builder
            .Services.CaptureRegisteredServices(s => builder.AddServiceDefaults())
            .ForTesting(new ToDisplayNameOptions(FullNames: true));

        await Verify(capture).UseSnapshotsDirectory();
    }

    [Fact]
    public async Task AddFluentValidationServicesTest()
    {
        var builder = WebApplication.CreateEmptyBuilder(new WebApplicationOptions());

        var capture = builder
            .Services.CaptureRegisteredServices(s => builder.AddFluentValidationServices())
            .ForTesting(new ToDisplayNameOptions(FullNames: true));

        await Verify(capture).UseSnapshotsDirectory();
    }

    [Fact]
    public async Task AddExceptionHandlingServicesTest()
    {
        var builder = WebApplication.CreateEmptyBuilder(new WebApplicationOptions());

        var capture = builder
            .Services.CaptureRegisteredServices(s => builder.AddExceptionHandlingServices())
            .ForTesting(new ToDisplayNameOptions(FullNames: true));

        await Verify(capture).UseSnapshotsDirectory();
    }

    [Fact]
    public async Task AddSchemaRegistryInfrastructureServicesTest()
    {
        var builder = WebApplication.CreateEmptyBuilder(new WebApplicationOptions());

        var capture = builder
            .Services.CaptureRegisteredServices(s =>
                builder.AddSchemaRegistryInfrastructureServices()
            )
            .ForTesting(new ToDisplayNameOptions(FullNames: true));

        await Verify(capture).UseSnapshotsDirectory();
    }

    [Fact]
    public async Task AddOpenApiServicesTest()
    {
        var builder = WebApplication.CreateEmptyBuilder(new WebApplicationOptions());

        builder.Configuration.AddInMemoryCollection(
            [new KeyValuePair<string, string?>("ConnectionStrings:database", "1234")]
        );

        var capture = builder
            .Services.CaptureRegisteredServices(s => builder.AddOpenApiServices())
            .ForTesting(new ToDisplayNameOptions(FullNames: true));

        await Verify(capture).UseSnapshotsDirectory();
    }

    [Fact]
    public async Task AddPostgresServicesTest()
    {
        var builder = WebApplication.CreateEmptyBuilder(new WebApplicationOptions());

        builder.Configuration.AddInMemoryCollection(
            [new KeyValuePair<string, string?>("ConnectionStrings:database", "1234")]
        );

        var capture = builder
            .Services.CaptureRegisteredServices(s => builder.AddPostgresServices())
            .ForTesting(new ToDisplayNameOptions(FullNames: true));

        await Verify(capture).UseSnapshotsDirectory();
    }

    [Fact]
    public async Task AddMartenServicesTest()
    {
        var builder = WebApplication.CreateEmptyBuilder(new WebApplicationOptions());

        var capture = builder
            .Services.CaptureRegisteredServices(s => builder.AddMartenServices())
            .ForTesting(new ToDisplayNameOptions(FullNames: true));

        await Verify(capture).UseSnapshotsDirectory();
    }

    [Fact]
    public async Task AddOrleansServicesTest()
    {
        var builder = WebApplication.CreateEmptyBuilder(new WebApplicationOptions());

        var capture = builder
            .Services.CaptureRegisteredServices(s => builder.AddOrleansServices())
            .ForTesting(new ToDisplayNameOptions(FullNames: true));

        await Verify(capture).UseSnapshotsDirectory();
    }
}
