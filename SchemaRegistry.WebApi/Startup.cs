using FluentValidation;
using JasperFx;
using JasperFx.CodeGeneration;
using JasperFx.Events;
using JasperFx.Events.Daemon;
using Marten;
using Marten.Services;
using Microsoft.OpenApi.Models;
using Pico.DependencyInjection;
using Pico.OpenTelemetry;
using Pico.WebApi;
using Scalar.AspNetCore;
using SchemaRegistry.Domain;
using SchemaRegistry.Infrastructure.NamespaceFeature;
using SchemaRegistry.WebApi.NamespaceFeature;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace SchemaRegistry.WebApi;

internal static class Startup
{
    public static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.LogRegisteredServices(
            "[***] ServiceDefaults services",
            _ => builder.AddServiceDefaults(),
            TextWriter.Null
        );

        builder.Services.LogRegisteredServices(
            "[***] FluentValidation services",
            services =>
                services
                    .AddValidatorsFromAssemblyContaining<SchemaRegistryDomain>(
                        includeInternalTypes: true
                    )
                    .AddFluentValidationAutoValidation(),
            Console.Out
        );

        builder.Services.LogRegisteredServices(
            "[***] ProblemDetails services",
            services =>
                services
                    .AddProblemDetails()
                    .AddCustomExceptionHandlerMiddleware()
                    .AddPicoExceptionsToProblemHandlers(),
            Console.Out
        );

        builder.Services.LogRegisteredServices(
            "[***] SchemaRegistry Infrastructure services",
            services => services.AddSchemaRegistryOrleansServices(),
            Console.Out
        );

        builder.Services.LogRegisteredServices(
            "[***] OpenAPI services",
            services =>
                services.AddOpenApi(options =>
                {
                    options.AddDocumentTransformer(
                        (document, _, _) =>
                        {
                            document.Info = new OpenApiInfo()
                            {
                                Title = "Schema Registry API",
                                Version = "v1",
                                Description = "API for managing schema registry",
                            };

                            return Task.CompletedTask;
                        }
                    );
                }),
            Console.Out
        );

        builder.Services.LogRegisteredServices(
            "[***] Postgres services",
            services =>
            {
                var connectionString =
                    builder.Configuration.GetConnectionString("database")
                    ?? throw new Exception("Connection string 'database' not found.");

                services.AddNpgsqlDataSource(connectionString);

                services.AddOpenTelemetrySources(nameof(Npgsql));
            },
            Console.Out
        );

        builder.Services.LogRegisteredServices(
            "[***] Marten services",
            services =>
            {
                services
                    .AddMarten(options =>
                    {
                        options.UseSystemTextJsonForSerialization();

                        options.AutoCreateSchemaObjects = AutoCreate.All;

                        options.DatabaseSchemaName = "marten";

                        options.Events.StreamIdentity = StreamIdentity.AsString;
                        options.Events.MetadataConfig.HeadersEnabled = true;
                        options.Events.MetadataConfig.CausationIdEnabled = true;
                        options.Events.MetadataConfig.CorrelationIdEnabled = true;
                        options.Events.MetadataConfig.UserNameEnabled = true;

                        options.OpenTelemetry.TrackConnections = TrackLevel.Normal;
                        options.OpenTelemetry.TrackEventCounters();
                    })
                    .UseNpgsqlDataSource()
                    .UseLightweightSessions()
                    .ApplyAllDatabaseChangesOnStartup()
                    .AddAsyncDaemon(DaemonMode.HotCold);

                services.CritterStackDefaults(_ => { });
                //services.CritterStackDefaults(options =>
                //{
                //    options.Development.GeneratedCodeMode = TypeLoadMode.Auto;
                //    options.Development.ResourceAutoCreate = AutoCreate.All;
                //    options.Development.AssertAllPreGeneratedTypesExist = true;
                //    options.Development.SourceCodeWritingEnabled = false;

                //    options.Production.GeneratedCodeMode = TypeLoadMode.Static;
                //    options.Production.ResourceAutoCreate = AutoCreate.None;
                //    options.Production.AssertAllPreGeneratedTypesExist = true;
                //    options.Production.SourceCodeWritingEnabled = false;
                //});

                services.AddOpenTelemetrySources(nameof(Marten));
            },
            Console.Out
        );

        builder.Services.LogRegisteredServices(
            "[***] Microsoft Orleans services",
            _ =>
                builder.Host.UseOrleans(silo =>
                {
                    silo.UseLocalhostClustering();
                }),
            TextWriter.Null
        );
    }

    public static void ConfigureApplication(WebApplication app)
    {
        app.MapDefaultEndpoints();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(
                "/docs",
                (options, context) =>
                {
                    options.Title = "Schema Registry API";
                }
            );
        }

        app.UseHttpsRedirection();

        app.UseExceptionToProblemMiddleware();

        app.MapSchemaRegistryEndpoints();
    }
}
