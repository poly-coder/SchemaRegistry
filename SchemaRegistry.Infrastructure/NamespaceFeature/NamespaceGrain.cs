using System;
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
        if (aggregate is not null)
        {
            throw new AlreadyExistsException(NamespaceMetadata.Name, Id);
        }

        await using var session = store.LightweightSession();

        var created = new NamespaceWasCreated(
            Name: command.Name,
            DisplayName: command.DisplayName,
            Description: command.Description,
            Documentation: command.Documentation
        );

        var createdEvent = session.Events.BuildEvent(created);

        var action = session.Events.StartStream<NamespaceAggregate>(Id, createdEvent);

        await session.SaveChangesAsync(cancel);

        await UpdateAggregate(session, action.Events, cancel);

        return new();
    }

    private async Task UpdateAggregate(
        IQuerySession session,
        IReadOnlyList<IEvent> events,
        CancellationToken cancel
    )
    {
        aggregate = await storeOptions
            .Projections.AggregatorFor<NamespaceAggregate>()
            .BuildAsync(events, session, aggregate, cancel);
    }
}
