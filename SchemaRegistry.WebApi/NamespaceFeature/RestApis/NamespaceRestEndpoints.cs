using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Pico.Domain.FluentValidation;
using SchemaRegistry.Application.NamespaceFeature;
using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.WebApi.NamespaceFeature.RestApis;

internal static class NamespaceRestEndpoints
{
    public const string NamespaceBasePath = "/ns";

    public static IEndpointRouteBuilder MapNamespaceEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup(NamespaceBasePath).WithTags("Namespace");

        var queries = group
            .MapGroup("")
            .ProducesValidationProblem()
            .ProducesProblem((int)HttpStatusCode.Forbidden)
            .ProducesProblem((int)HttpStatusCode.InternalServerError);

        var commands = group
            .MapGroup("")
            .ProducesValidationProblem()
            .ProducesProblem((int)HttpStatusCode.Conflict)
            .ProducesProblem((int)HttpStatusCode.Forbidden)
            .ProducesProblem((int)HttpStatusCode.InternalServerError);

        queries
            .MapGet("/{name}", GetNamespaceById)
            .WithName(nameof(GetNamespaceById))
            .WithSummary("Get a namespace by name")
            .WithDescription("Gets a namespace by name from the schema registry.")
            .Produces<GetNamespaceByIdResult>();

        commands
            .MapPost("/{name}", CreateNamespace)
            .WithName(nameof(CreateNamespace))
            .WithSummary("Create a new namespace")
            .WithDescription("Creates a new namespace in the schema registry.")
            .Produces<NamespaceCommandResult>(StatusCodes.Status201Created);

        commands
            .MapPut("/{name}/descriptions", UpdateNamespaceDescriptions)
            .WithName(nameof(UpdateNamespaceDescriptions))
            .WithSummary("Update the descriptions of a namespace")
            .WithDescription("Updates the descriptions of a namespace in the schema registry.")
            .Produces<NamespaceCommandResult>();

        commands
            .MapPut("/{name}/documentation", UpdateNamespaceDocumentation)
            .WithName(nameof(UpdateNamespaceDocumentation))
            .WithSummary("Update the documentation of a namespace")
            .WithDescription("Updates the documentation of a namespace in the schema registry.")
            .Produces<NamespaceCommandResult>();

        commands
            .MapPut("/{name}/restore", RestoreNamespace)
            .WithName(nameof(RestoreNamespace))
            .WithSummary("Restore a deleted namespace")
            .WithDescription("Restores a deleted namespace in the schema registry.")
            .Produces<NamespaceCommandResult>();

        commands
            .MapDelete("/{name}", DeleteNamespace)
            .WithName(nameof(DeleteNamespace))
            .WithSummary("Delete a new namespace")
            .WithDescription(
                "Deletes a namespace from the schema registry. Indicate whether to delete permanently or not."
            )
            .Produces<NamespaceCommandResult>();

        return endpoints;
    }

    internal static async Task<GetNamespaceByIdResult> GetNamespaceById(
        [FromRoute(Name = "name")] string name,
        [FromServices] IGetNamespaceByIdService service,
        [FromServices] IValidator<GetNamespaceByIdQuery> validator,
        CancellationToken cancel,
        [FromQuery(Name = "deleted")] bool deleted = false
    )
    {
        var command = await new GetNamespaceByIdQuery(name, deleted)
            .Coerce()
            .ValidateWithAsync(validator, cancel);

        var result = await service.GetNamespaceByIdAsync(command, cancel);

        return result.MapToRestApiResult();
    }

    internal static async Task<NamespaceCommandResult> CreateNamespace(
        [FromRoute(Name = "name")] string name,
        [FromBody] CreateNamespaceCommand body,
        [FromServices] ICreateNamespaceService service,
        [FromServices] IValidator<Domain.NamespaceFeature.CreateNamespaceCommand> validator,
        CancellationToken cancel
    )
    {
        var command = await body.MapToCommand(name).Coerce().ValidateWithAsync(validator, cancel);

        var result = await service.CreateNamespaceAsync(command, cancel);

        return result.MapToRestApiResult();
    }

    internal static async Task<NamespaceCommandResult> UpdateNamespaceDescriptions(
        [FromRoute(Name = "name")] string name,
        [FromBody] UpdateNamespaceDescriptionsCommand body,
        [FromServices] IUpdateNamespaceDescriptionsService service,
        [FromServices]
            IValidator<Domain.NamespaceFeature.UpdateNamespaceDescriptionsCommand> validator,
        CancellationToken cancel
    )
    {
        var command = await body.MapToCommand(name).Coerce().ValidateWithAsync(validator, cancel);

        var result = await service.UpdateNamespaceDescriptionsAsync(command, cancel);

        return result.MapToRestApiResult();
    }

    internal static async Task<NamespaceCommandResult> UpdateNamespaceDocumentation(
        [FromRoute(Name = "name")] string name,
        [FromBody] UpdateNamespaceDocumentationCommand body,
        [FromServices] IUpdateNamespaceDocumentationService service,
        [FromServices]
            IValidator<Domain.NamespaceFeature.UpdateNamespaceDocumentationCommand> validator,
        CancellationToken cancel
    )
    {
        var command = await body.MapToCommand(name).Coerce().ValidateWithAsync(validator, cancel);

        var result = await service.UpdateNamespaceDocumentationAsync(command, cancel);

        return result.MapToRestApiResult();
    }

    internal static async Task<NamespaceCommandResult> RestoreNamespace(
        [FromRoute(Name = "name")] string name,
        [FromServices] IRestoreNamespaceService service,
        [FromServices] IValidator<RestoreNamespaceCommand> validator,
        CancellationToken cancel
    )
    {
        var command = await new RestoreNamespaceCommand(name)
            .Coerce()
            .ValidateWithAsync(validator, cancel);

        var result = await service.RestoreNamespaceAsync(command, cancel);

        return result.MapToRestApiResult();
    }

    internal static async Task<NamespaceCommandResult> DeleteNamespace(
        [FromRoute(Name = "name")] string name,
        [FromServices] IDeleteNamespaceService service,
        [FromServices] IValidator<DeleteNamespaceCommand> validator,
        CancellationToken cancel
    )
    {
        var command = new DeleteNamespaceCommand(name).Coerce().ValidateWith(validator);

        var result = await service.DeleteNamespaceAsync(command, cancel);

        return result.MapToRestApiResult();
    }
}
