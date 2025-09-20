using SchemaRegistry.Application.NamespaceFeature;
using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.Infrastructure.NamespaceFeature;

internal sealed class NamespaceGrainService(IGrainFactory grains)
    : ICreateNamespaceService,
        IUpdateNamespaceDescriptionsService,
        IDeleteNamespaceService,
        IRestoreNamespaceService,
        IGetNamespaceByIdService
{
    public async Task<CreateNamespaceCommandResult> CreateNamespaceAsync(
        CreateNamespaceCommand command,
        CancellationToken cancel = default
    )
    {
        var grain = grains.GetGrain<INamespaceGrain>(command.Name);

        var input = command.MapToInput();

        var output = await grain.CreateNamespace(input, cancel);

        return output.MapToResult();
    }

    public async Task<UpdateNamespaceDescriptionsCommandResult> UpdateNamespaceDescriptionsAsync(
        UpdateNamespaceDescriptionsCommand command,
        CancellationToken cancel = default
    )
    {
        var grain = grains.GetGrain<INamespaceGrain>(command.Name);

        var input = command.MapToInput();

        var output = await grain.UpdateNamespaceDescriptions(input, cancel);

        return output.MapToResult();
    }

    public async Task<DeleteNamespaceCommandResult> DeleteNamespaceAsync(
        DeleteNamespaceCommand command,
        CancellationToken cancel = default
    )
    {
        var grain = grains.GetGrain<INamespaceGrain>(command.Name);

        var input = command.MapToInput();

        var output = await grain.DeleteNamespace(input, cancel);

        return output.MapToResult();
    }

    public async Task<RestoreNamespaceCommandResult> RestoreNamespaceAsync(
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

    internal static CreateNamespaceCommandResult MapToResult(this CreateNamespaceOutput output) =>
        new();

    internal static UpdateNamespaceDescriptionsInput MapToInput(
        this UpdateNamespaceDescriptionsCommand command
    ) =>
        new(
            DisplayName: command.DisplayName,
            Description: command.Description,
            Documentation: command.Documentation
        );

    internal static UpdateNamespaceDescriptionsCommandResult MapToResult(
        this UpdateNamespaceDescriptionsOutput output
    ) => new();

    internal static DeleteNamespaceInput MapToInput(this DeleteNamespaceCommand command) =>
        new(Permanently: command.Permanently);

    internal static DeleteNamespaceCommandResult MapToResult(this DeleteNamespaceOutput output) =>
        new(PermanentlyDeleted: output.PermanentlyDeleted);

    internal static RestoreNamespaceInput MapToInput(this RestoreNamespaceCommand command) => new();

    internal static RestoreNamespaceCommandResult MapToResult(this RestoreNamespaceOutput output) =>
        new();

    internal static GetNamespaceByIdInput MapToInput(this GetNamespaceByIdQuery query) =>
        new(query.Deleted);

    internal static GetNamespaceByIdQueryResult MapToResult(this GetNamespaceByIdOutput output) =>
        new(output.Details.MapToDomain(), output.Operations.MapToDomain());
}
