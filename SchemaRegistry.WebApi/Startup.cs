using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

namespace SchemaRegistry.WebApi;

internal static class Startup
{
    public static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();

        builder.Services.AddOpenApi(options =>
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
        });
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

        var api = app.MapGroup("/api");
    }
}
