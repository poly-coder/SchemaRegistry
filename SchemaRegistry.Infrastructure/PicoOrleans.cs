using System.Diagnostics.CodeAnalysis;
using ImTools;
using JasperFx.Events;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Pico.Domain.Errors;

namespace Pico.Orleans;

public abstract class PicoMartenAggregateGrain<TAggregate>(IServiceProvider provider) : Grain
    where TAggregate : class
{
    protected IDocumentStore Store { get; } = provider.GetRequiredService<IDocumentStore>();
    protected StoreOptions StoreOptions { get; } = provider.GetRequiredService<StoreOptions>();

    protected TAggregate? Aggregate { get; private set; }
    protected long Version { get; private set; }

    protected abstract string EntityName { get; }
    protected string EntityId => this.GetPrimaryKeyString();

    protected void CheckNew()
    {
        if (Aggregate is not null)
        {
            throw new AlreadyExistsException(EntityName, EntityId);
        }
    }

    [MemberNotNull(nameof(Aggregate))]
    protected void CheckExists(bool deleted = false)
    {
        if (Aggregate is null || deleted && IsDeleted(Aggregate))
        {
            throw new EntityNotFoundException(EntityName, EntityId);
        }
    }

    [MemberNotNull(nameof(Aggregate))]
    protected void CheckNotDeleted()
    {
        if (Aggregate is null || IsDeleted(Aggregate))
        {
            throw new EntityNotFoundException(EntityName, EntityId);
        }
    }

    protected abstract bool IsDeleted(TAggregate aggregate);

    protected async Task RefreshAggregate(
        IQuerySession session,
        IReadOnlyList<IEvent> events,
        CancellationToken cancel
    )
    {
        Aggregate = await StoreOptions
            .Projections.AggregatorFor<TAggregate>()
            .BuildAsync(events, session, Aggregate, cancel);

        if (events.Count > 0)
        {
            Version = events[^1].Version;
        }
    }

    protected async Task CreateOperation(
        Func<IEnumerable<object>> createEvents,
        CancellationToken cancel
    )
    {
        CheckNew();

        ArgumentNullException.ThrowIfNull(createEvents);

        var events = createEvents();

        await CreateOperationInternal(events, cancel);
    }

    protected async Task CreateOperation(IEnumerable<object> events, CancellationToken cancel)
    {
        CheckNew();

        ArgumentNullException.ThrowIfNull(events);

        await CreateOperationInternal(events, cancel);
    }

    private async Task CreateOperationInternal(IEnumerable<object> events, CancellationToken cancel)
    {
        await using var session = Store.LightweightSession();

        var action = session.Events.StartStream<TAggregate>(EntityId, events);

        await session.SaveChangesAsync(cancel);

        await RefreshAggregate(session, action.Events, cancel);
    }

    protected async Task<bool> UpdateOperation(
        Func<TAggregate, IEnumerable<object>> createEvents,
        CancellationToken cancel
    )
    {
        CheckNotDeleted();

        return await UpdateOperationInternal(createEvents, cancel);
    }

    protected async Task<bool> UpdateOperation(
        bool deleted,
        Func<TAggregate, IEnumerable<object>> createEvents,
        CancellationToken cancel
    )
    {
        CheckExists(deleted);

        return await UpdateOperationInternal(createEvents, cancel);
    }

    private async Task<bool> UpdateOperationInternal(
        Func<TAggregate, IEnumerable<object>> createEvents,
        CancellationToken cancel
    )
    {
        ArgumentNullException.ThrowIfNull(Aggregate);
        ArgumentNullException.ThrowIfNull(createEvents);

        await using var session = Store.LightweightSession();

        var events = createEvents(Aggregate).ToArray();

        if (events.Length > 0)
        {
            var action = session.Events.Append(EntityId, events);

            await session.SaveChangesAsync(cancel);

            await RefreshAggregate(session, action.Events, cancel);

            return true;
        }

        return false;
    }

    public override async Task OnActivateAsync(CancellationToken cancel)
    {
        await base.OnActivateAsync(cancel);

        await using var session = Store.QuerySession();

        var events = await session.Events.FetchStreamAsync(EntityId, token: cancel);

        // TODO: Snapshot loading

        await RefreshAggregate(session, events, cancel);
    }
}
