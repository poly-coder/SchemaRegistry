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

    public async Task<CreateNamespaceOutput> CreateNamespace(
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

        return new();
    }

    public async Task<UpdateNamespaceDescriptionsOutput> UpdateNamespaceDescriptions(
        UpdateNamespaceDescriptionsInput command,
        CancellationToken cancel
    )
    {
        ValidateExistingAggregate(deleted: false);

        await using var session = store.LightweightSession();

        var created = new NamespaceDescriptionsWereUpdated(
            DisplayName: command.DisplayName,
            Description: command.Description,
            Documentation: command.Documentation
        );

        var action = session.Events.Append(Id, version, created);

        await session.SaveChangesAsync(cancel);

        await UpdateAggregate(session, action, cancel);

        return new();
    }

    public async Task<DeleteNamespaceOutput> DeleteNamespace(
        DeleteNamespaceInput command,
        CancellationToken cancel
    )
    {
        ValidateExistingAggregate(deleted: true);

        await using var session = store.LightweightSession();

        StreamAction action;

        if (command.Permanently)
        {
            if (aggregate.DeletedAt is null)
            {
                throw new OperationConflictException(
                    NamespaceMetadata.Name,
                    Id,
                    nameof(DeleteNamespace),
                    "Cannot permanently delete an active namespace. Please, soft delete the namespace first."
                );
            }

            var restored = new NamespaceWasPermanentlyDeleted();

            action = session.Events.Append(Id, version, restored);
        }
        else
        {
            if (aggregate.DeletedAt is not null)
            {
                return new(PermanentlyDeleted: false);
            }

            var softDeleted = new NamespaceWasSoftDeleted();

            action = session.Events.Append(Id, version, softDeleted);
        }

        await session.SaveChangesAsync(cancel);

        await UpdateAggregate(session, action, cancel);

        return new(PermanentlyDeleted: command.Permanently);
    }

    public async Task<RestoreNamespaceOutput> RestoreNamespace(
        RestoreNamespaceInput command,
        CancellationToken cancel
    )
    {
        ValidateExistingAggregate(deleted: true);

        await using var session = store.LightweightSession();

        if (aggregate.DeletedAt is null)
        {
            return new();
        }

        var restored = new NamespaceWasRestored();

        var action = session.Events.Append(Id, version, restored);

        await session.SaveChangesAsync(cancel);

        await UpdateAggregate(session, action, cancel);

        return new();
    }

    public Task<GetNamespaceByIdOutput> GetNamespaceById(
        GetNamespaceByIdInput command,
        CancellationToken cancel
    )
    {
        ValidateExistingAggregate(command.Deleted);

        var details = aggregate.MapToDetails();

        var operations = new NamespaceOperationsData(
            CanUpdateDescriptions: !details.IsDeleted,
            CanDelete: !details.IsDeleted,
            CanRestore: details.IsDeleted,
            CanDeletePermanently: details.IsDeleted
        );

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
