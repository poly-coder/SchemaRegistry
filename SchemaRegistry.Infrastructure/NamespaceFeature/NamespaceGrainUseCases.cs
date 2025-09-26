using SchemaRegistry.Application.NamespaceFeature;

namespace SchemaRegistry.Infrastructure.NamespaceFeature;

internal sealed class NamespaceGrainUseCases(IGrainFactory grains)
    : ICreateNamespaceUseCase,
        IUpdateNamespaceDescriptionsUseCase,
        IUpdateNamespaceDocumentationUseCase,
        IDeleteNamespaceUseCase,
        IRestoreNamespaceUseCase,
        IGetNamespaceByIdUseCase
{
    public async Task<Domain.NamespaceFeature.NamespaceCommandResult> CreateNamespaceAsync(
        Domain.NamespaceFeature.CreateNamespaceCommand command,
        CancellationToken cancel = default
    )
    {
        var grain = grains.GetGrain<INamespaceGrain>(command.Name);

        var input = command.MapToOrleans();

        var output = await grain.CreateNamespace(input, cancel);

        return output.MapToDomain();
    }

    public async Task<Domain.NamespaceFeature.NamespaceCommandResult> UpdateNamespaceDescriptionsAsync(
        Domain.NamespaceFeature.UpdateNamespaceDescriptionsCommand command,
        CancellationToken cancel = default
    )
    {
        var grain = grains.GetGrain<INamespaceGrain>(command.Name);

        var input = command.MapToOrleans();

        var output = await grain.UpdateNamespaceDescriptions(input, cancel);

        return output.MapToDomain();
    }

    public async Task<Domain.NamespaceFeature.NamespaceCommandResult> UpdateNamespaceDocumentationAsync(
        Domain.NamespaceFeature.UpdateNamespaceDocumentationCommand command,
        CancellationToken cancel = default
    )
    {
        var grain = grains.GetGrain<INamespaceGrain>(command.Name);

        var input = command.MapToOrleans();

        var output = await grain.UpdateNamespaceDocumentation(input, cancel);

        return output.MapToDomain();
    }

    public async Task<Domain.NamespaceFeature.NamespaceCommandResult> DeleteNamespaceAsync(
        Domain.NamespaceFeature.DeleteNamespaceCommand command,
        CancellationToken cancel = default
    )
    {
        var grain = grains.GetGrain<INamespaceGrain>(command.Name);

        var input = command.MapToOrleans();

        var output = await grain.DeleteNamespace(input, cancel);

        return output.MapToDomain();
    }

    public async Task<Domain.NamespaceFeature.NamespaceCommandResult> RestoreNamespaceAsync(
        Domain.NamespaceFeature.RestoreNamespaceCommand command,
        CancellationToken cancel = default
    )
    {
        var grain = grains.GetGrain<INamespaceGrain>(command.Name);

        var input = command.MapToOrleans();

        var output = await grain.RestoreNamespace(input, cancel);

        return output.MapToDomain();
    }

    public async Task<Domain.NamespaceFeature.GetNamespaceByIdQueryResult> GetNamespaceByIdAsync(
        Domain.NamespaceFeature.GetNamespaceByIdQuery query,
        CancellationToken cancel = default
    )
    {
        var grain = grains.GetGrain<INamespaceGrain>(query.Name);

        var input = query.MapToOrleans();

        var output = await grain.GetNamespaceById(input, cancel);

        return output.MapToDomain();
    }
}
