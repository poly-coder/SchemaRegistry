using SchemaRegistry.Application.NamespaceFeature;
using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.Infrastructure.NamespaceFeature;

internal sealed class NamespaceGrainService(IGrainFactory grains)
    : ICreateNamespaceService,
        IUpdateNamespaceDescriptionsService,
        IUpdateNamespaceDocumentationService,
        IDeleteNamespaceService,
        IRestoreNamespaceService,
        IGetNamespaceByIdService
{
    public async Task<NamespaceCommandResult> CreateNamespaceAsync(
        CreateNamespaceCommand command,
        CancellationToken cancel = default
    )
    {
        var grain = grains.GetGrain<INamespaceGrain>(command.Name);

        var input = command.MapToInput();

        var output = await grain.CreateNamespace(input, cancel);

        return output.MapToResult();
    }

    public async Task<NamespaceCommandResult> UpdateNamespaceDescriptionsAsync(
        UpdateNamespaceDescriptionsCommand command,
        CancellationToken cancel = default
    )
    {
        var grain = grains.GetGrain<INamespaceGrain>(command.Name);

        var input = command.MapToInput();

        var output = await grain.UpdateNamespaceDescriptions(input, cancel);

        return output.MapToResult();
    }

    public async Task<NamespaceCommandResult> UpdateNamespaceDocumentationAsync(
        UpdateNamespaceDocumentationCommand command,
        CancellationToken cancel = default
    )
    {
        var grain = grains.GetGrain<INamespaceGrain>(command.Name);

        var input = command.MapToInput();

        var output = await grain.UpdateNamespaceDocumentation(input, cancel);

        return output.MapToResult();
    }

    public async Task<NamespaceCommandResult> DeleteNamespaceAsync(
        DeleteNamespaceCommand command,
        CancellationToken cancel = default
    )
    {
        var grain = grains.GetGrain<INamespaceGrain>(command.Name);

        var input = command.MapToInput();

        var output = await grain.DeleteNamespace(input, cancel);

        return output.MapToResult();
    }

    public async Task<NamespaceCommandResult> RestoreNamespaceAsync(
        RestoreNamespaceCommand command,
        CancellationToken cancel = default
    )
    {
        var grain = grains.GetGrain<INamespaceGrain>(command.Name);

        var input = command.MapToInput();

        var output = await grain.RestoreNamespace(input, cancel);

        return output.MapToResult();
    }

    public async Task<GetNamespaceByIdQueryResult> GetNamespaceByIdAsync(
        GetNamespaceByIdQuery query,
        CancellationToken cancel = default
    )
    {
        var grain = grains.GetGrain<INamespaceGrain>(query.Name);

        var input = query.MapToInput();

        var output = await grain.GetNamespaceById(input, cancel);

        return output.MapToResult();
    }
}

// Mapper

internal static class NamespaceGrainServiceMapper
{
    internal static CreateNamespaceInput MapToInput(this CreateNamespaceCommand command) =>
        new(
            DisplayName: command.DisplayName,
            Description: command.Description,
            Documentation: command.Documentation
        );

    internal static UpdateNamespaceDescriptionsInput MapToInput(
        this UpdateNamespaceDescriptionsCommand command
    ) => new(DisplayName: command.DisplayName, Description: command.Description);

    internal static UpdateNamespaceDocumentationInput MapToInput(
        this UpdateNamespaceDocumentationCommand command
    ) => new(Documentation: command.Documentation);

    internal static DeleteNamespaceInput MapToInput(this DeleteNamespaceCommand command) => new();

    internal static RestoreNamespaceInput MapToInput(this RestoreNamespaceCommand command) => new();

    internal static NamespaceCommandResult MapToResult(this NamespaceCommandOutput output) =>
        new(Updated: output.Updated);

    internal static GetNamespaceByIdInput MapToInput(this GetNamespaceByIdQuery query) =>
        new(Deleted: query.Deleted);

    internal static GetNamespaceByIdQueryResult MapToResult(this GetNamespaceByIdOutput output) =>
        new(output.Details.MapToDomain(), output.Operations.MapToDomain());
}
