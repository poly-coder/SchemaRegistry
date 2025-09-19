using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Pico.Domain.FluentValidation;
using SchemaRegistry.Application.NamespaceFeature;
using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.WebApi.NamespaceFeature;

internal static class CreateNamespaceRestEndpoints
{
    public const string OperationName = "CreateNamespace";

    public static IEndpointRouteBuilder MapCreateNamespaceEndpoints(
        this IEndpointRouteBuilder endpoints
    )
    {
        endpoints
            .MapPost("/", HandlePost)
            .WithName(OperationName)
            .WithSummary("Create a new namespace")
            .WithDescription("Creates a new namespace in the schema registry.")
            .Produces<CreateNamespaceResponseBody>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem((int)HttpStatusCode.Conflict);

        return endpoints;
    }

    public static async Task<CreateNamespaceResponseBody> HandlePost(
        [FromBody] CreateNamespaceRequestBody body,
        [FromServices] ICreateNamespaceService service,
        [FromServices] IValidator<CreateNamespaceCommand> validator,
        CancellationToken cancel
    )
    {
        var command = await body.MapToCommand().Coerce().ValidateWithAsync(validator, cancel);

        var result = await service.CreateNamespaceAsync(command, cancel);

        return result.MapToResponseBody();
    }
}

// Models

public sealed record CreateNamespaceRequestBody(
    string Name,
    string? DisplayName = null,
    string? Description = null,
    string? Documentation = null
);

public sealed record CreateNamespaceResponseBody();

// Mapper

public static class CreateNamespaceRestMapper
{
    public static CreateNamespaceCommand MapToCommand(this CreateNamespaceRequestBody body)
    {
        return new CreateNamespaceCommand(
            body.Name,
            body.DisplayName,
            body.Description,
            body.Documentation
        );
    }

    public static CreateNamespaceResponseBody MapToResponseBody(
        this CreateNamespaceCommandResult result
    )
    {
        return new();
    }
}
