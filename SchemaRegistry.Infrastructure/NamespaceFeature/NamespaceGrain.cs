using System.Diagnostics.CodeAnalysis;
using JasperFx.Events;
using Marten;
using Pico.Domain.Errors;
using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.Infrastructure.NamespaceFeature;

public sealed class NamespaceGrain(IDocumentStore store, StoreOptions storeOptions)
    : Grain,
        INamespaceGrain
{
    private NamespaceAggregate? aggregate;
    private long version;

    private string Id => this.GetPrimaryKeyString();

    public override async Task OnActivateAsync(CancellationToken cancel)
    {
        await base.OnActivateAsync(cancel);

        await using var session = store.QuerySession();

        aggregate = await session.Events.AggregateStreamAsync<NamespaceAggregate>(
            Id,
            token: cancel
        );
    }

    public async Task<NamespaceCommandOutput> CreateNamespace(
        CreateNamespaceInput command,
        CancellationToken cancel
    )
    {
        ValidateNewAggregate();

        await using var session = store.LightweightSession();

        var created = new NamespaceWasCreated(
            DisplayName: command.DisplayName,
            Description: command.Description,
            Documentation: command.Documentation
        );

        var action = session.Events.StartStream<NamespaceAggregate>(Id, created);

        await session.SaveChangesAsync(cancel);

        await UpdateAggregate(session, action, cancel);

        return new(Updated: true);
    }

    public async Task<NamespaceCommandOutput> UpdateNamespaceDescriptions(
        UpdateNamespaceDescriptionsInput command,
        CancellationToken cancel
    )
    {
        ValidateExistingAggregate(deleted: false);

        await using var session = store.LightweightSession();

        if (
            StringComparer.Ordinal.Equals(command.DisplayName, aggregate.DisplayName)
            && StringComparer.Ordinal.Equals(command.Description, aggregate.Description)
        )
        {
            return new(Updated: false);
        }

        var updated = new NamespaceDescriptionsWereUpdated(
            DisplayName: command.DisplayName,
            Description: command.Description
        );

        var action = session.Events.Append(Id, version, updated);

        await session.SaveChangesAsync(cancel);

        await UpdateAggregate(session, action, cancel);

        return new(Updated: true);
    }

    public async Task<NamespaceCommandOutput> UpdateNamespaceDocumentation(
        UpdateNamespaceDocumentationInput command,
        CancellationToken cancel
    )
    {
        ValidateExistingAggregate(deleted: false);

        await using var session = store.LightweightSession();

        if (StringComparer.Ordinal.Equals(command.Documentation, aggregate.Documentation))
        {
            return new(Updated: false);
        }

        var updated = new NamespaceDocumentationWasUpdated(Documentation: command.Documentation);

        var action = session.Events.Append(Id, version, updated);

        await session.SaveChangesAsync(cancel);

        await UpdateAggregate(session, action, cancel);

        return new(Updated: true);
    }

    public async Task<NamespaceCommandOutput> DeleteNamespace(
        DeleteNamespaceInput command,
        CancellationToken cancel
    )
    {
        ValidateExistingAggregate(deleted: true);

        await using var session = store.LightweightSession();

        if (aggregate.Status is NamespaceStatus.Deleted)
        {
            return new(Updated: false);
        }

        var deleted = new NamespaceWasDeleted();

        var action = session.Events.Append(Id, version, deleted);

        await session.SaveChangesAsync(cancel);

        await UpdateAggregate(session, action, cancel);

        return new(Updated: true);
    }

    public async Task<NamespaceCommandOutput> RestoreNamespace(
        RestoreNamespaceInput command,
        CancellationToken cancel
    )
    {
        ValidateExistingAggregate(deleted: true);

        await using var session = store.LightweightSession();

        if (aggregate.Status is NamespaceStatus.Active)
        {
            return new(Updated: false);
        }

        var restored = new NamespaceWasRestored();

        var action = session.Events.Append(Id, version, restored);

        await session.SaveChangesAsync(cancel);

        await UpdateAggregate(session, action, cancel);

        return new(Updated: true);
    }

    public Task<GetNamespaceByIdOutput> GetNamespaceById(
        GetNamespaceByIdInput command,
        CancellationToken cancel
    )
    {
        ValidateExistingAggregate(command.Deleted);

        var details = aggregate.MapToDetails();

        var operations = aggregate.MapToOperations();

        return Task.FromResult(new GetNamespaceByIdOutput(details, operations));
    }

    private void ValidateNewAggregate()
    {
        if (aggregate is not null)
        {
            throw new AlreadyExistsException(NamespaceMetadata.Name, Id);
        }
    }

    [MemberNotNull(nameof(aggregate))]
    private void ValidateExistingAggregate(bool deleted)
    {
        if (aggregate is null || (!deleted && aggregate.DeletedAt is not null))
        {
            throw new EntityNotFoundException(NamespaceMetadata.Name, Id);
        }
    }

    private async Task UpdateAggregate(
        IQuerySession session,
        StreamAction action,
        CancellationToken cancel
    )
    {
        aggregate = await storeOptions
            .Projections.AggregatorFor<NamespaceAggregate>()
            .BuildAsync(action.Events, session, aggregate, cancel);

        version = action.Version;
    }
}
