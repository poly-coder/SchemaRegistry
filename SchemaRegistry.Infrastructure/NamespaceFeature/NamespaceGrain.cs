using Pico.Domain.Errors;
using Pico.Orleans;

namespace SchemaRegistry.Infrastructure.NamespaceFeature;

public sealed class NamespaceGrain(IServiceProvider provider)
    : PicoMartenAggregateGrain<NamespaceAggregate>(provider),
        INamespaceGrain
{
    public async Task<NamespaceCommandResult> CreateNamespace(
        CreateNamespaceCommand command,
        CancellationToken cancel
    )
    {
        await CreateOperation(command.GetEvents, cancel);

        return new(Updated: true);
    }

    public async Task<NamespaceCommandResult> UpdateNamespaceDescriptions(
        UpdateNamespaceDescriptionsCommand command,
        CancellationToken cancel
    )
    {
        var updated = await UpdateOperation(command.GetEvents, cancel);

        return new(Updated: updated);
    }

    public async Task<NamespaceCommandResult> UpdateNamespaceDocumentation(
        UpdateNamespaceDocumentationCommand command,
        CancellationToken cancel
    )
    {
        var updated = await UpdateOperation(command.GetEvents, cancel);

        return new(Updated: updated);
    }

    public async Task<NamespaceCommandResult> DeleteNamespace(
        DeleteNamespaceCommand command,
        CancellationToken cancel
    )
    {
        var updated = await UpdateOperation(true, command.GetEvents, cancel);

        return new(Updated: updated);
    }

    public async Task<NamespaceCommandResult> RestoreNamespace(
        RestoreNamespaceCommand command,
        CancellationToken cancel
    )
    {
        var updated = await UpdateOperation(true, command.GetEvents, cancel);

        return new(Updated: updated);
    }

    public Task<GetNamespaceByIdQueryResult> GetNamespaceById(
        GetNamespaceByIdQuery query,
        CancellationToken cancel
    )
    {
        CheckExists(query.Deleted);

        var info = query.GetDetailsInfo(Aggregate);

        return Task.FromResult(new GetNamespaceByIdQueryResult(info));
    }

    protected override string EntityName => Domain.NamespaceFeature.NamespaceMetadata.Name;

    protected override bool IsDeleted(NamespaceAggregate aggregate) =>
        aggregate.DeletedAt is not null;
}
