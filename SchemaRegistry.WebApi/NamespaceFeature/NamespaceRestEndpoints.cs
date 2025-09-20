using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Pico.Domain.FluentValidation;
using SchemaRegistry.Application.NamespaceFeature;
using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.WebApi.NamespaceFeature;

internal static class NamespaceRestEndpoints
{
    public const string NamespaceBasePath = "/ns";

    public static IEndpointRouteBuilder MapNamespaceEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup(NamespaceBasePath).WithTags("Namespace");

        var commands = group
            .MapGroup("")
            .ProducesValidationProblem()
            .ProducesProblem((int)HttpStatusCode.Conflict)
            .ProducesProblem((int)HttpStatusCode.Forbidden);

        commands
            .MapPost("/{name}", CreateNamespace)
            .WithName(nameof(CreateNamespace))
            .WithSummary("Create a new namespace")
            .WithDescription("Creates a new namespace in the schema registry.")
            .Produces<CreateNamespaceResponseBody>(StatusCodes.Status201Created);

        commands
            .MapPut("/{name}/descriptions", UpdateNamespaceDescriptions)
            .WithName(nameof(UpdateNamespaceDescriptions))
            .WithSummary("Update the descriptions of a namespace")
            .WithDescription("Updates the descriptions of a namespace in the schema registry.")
            .Produces<UpdateNamespaceDescriptionsResponseBody>();

        commands
            .MapPut("/{name}/restore", RestoreNamespace)
            .WithName(nameof(RestoreNamespace))
            .WithSummary("Restore a deleted namespace")
            .WithDescription("Restores a deleted namespace in the schema registry.")
            .Produces<RestoreNamespaceResponseBody>();

        commands
            .MapDelete("/{name}", DeleteNamespace)
            .WithName(nameof(DeleteNamespace))
            .WithSummary("Delete a new namespace")
            .WithDescription(
                "Deletes a namespace from the schema registry. Indicate whether to delete permanently or not."
            )
            .Produces<DeleteNamespaceResponseBody>();

        return endpoints;
    }

    internal static async Task<CreateNamespaceResponseBody> CreateNamespace(
        [FromRoute(Name = "name")] string name,
        [FromBody] CreateNamespaceRequestBody body,
        [FromServices] ICreateNamespaceService service,
        [FromServices] IValidator<CreateNamespaceCommand> validator,
        CancellationToken cancel
    )
    {
        var command = await body.MapToCommand(name).Coerce().ValidateWithAsync(validator, cancel);

        var result = await service.CreateNamespaceAsync(command, cancel);

        return result.MapToResponseBody();
    }

    internal static async Task<UpdateNamespaceDescriptionsResponseBody> UpdateNamespaceDescriptions(
        [FromRoute(Name = "name")] string name,
        [FromBody] UpdateNamespaceDescriptionsRequestBody body,
        [FromServices] IUpdateNamespaceDescriptionsService service,
        [FromServices] IValidator<UpdateNamespaceDescriptionsCommand> validator,
        CancellationToken cancel
    )
    {
        var command = await body.MapToCommand(name).Coerce().ValidateWithAsync(validator, cancel);

        var result = await service.UpdateNamespaceDescriptionsAsync(command, cancel);

        return result.MapToResponseBody();
    }

    internal static async Task<RestoreNamespaceResponseBody> RestoreNamespace(
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

        return result.MapToResponseBody();
    }

    internal static async Task<DeleteNamespaceResponseBody> DeleteNamespace(
        [FromRoute(Name = "name")] string name,
        [FromQuery(Name = "permanently")] bool permanently,
        [FromServices] IDeleteNamespaceService service,
        [FromServices] IValidator<DeleteNamespaceCommand> validator,
        CancellationToken cancel
    )
    {
        var command = new DeleteNamespaceCommand(name, permanently)
            .Coerce()
            .ValidateWith(validator);

        var result = await service.DeleteNamespaceAsync(command, cancel);

        return result.MapToResponseBody();
    }
}
