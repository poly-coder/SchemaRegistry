using FluentValidation;
using Microsoft.OpenApi.Models;
using Pico.DependencyInjection;
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
                    .AddExceptionToProblemHandler<FluentValidationExceptionToProblemHandler>(),
            Console.Out
        );

        builder.Services.LogRegisteredServices(
            "[***] SchemaRegistry Infrastructure services",
            services => services.AddSchemaRegistryInfrastructureServices(),
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
