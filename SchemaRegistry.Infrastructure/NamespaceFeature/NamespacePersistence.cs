using JasperFx.Events;
using Marten.Schema;

namespace SchemaRegistry.Infrastructure.NamespaceFeature;

public sealed record NamespaceAggregate(
    [property: Identity] string Name,
    string? DisplayName,
    string? Description,
    string? Documentation,
    DateTimeOffset CreatedAt,
    DateTimeOffset ModifiedAt,
    DateTimeOffset? DeletedAt = null
)
{
    public static NamespaceAggregate Create(IEvent<NamespaceWasCreated> @event) =>
        new(
            Name: @event.StreamKey!,
            DisplayName: @event.Data.DisplayName,
            Description: @event.Data.Description,
            Documentation: @event.Data.Documentation,
            CreatedAt: @event.Timestamp,
            ModifiedAt: @event.Timestamp
        );

    public static NamespaceAggregate Apply(
        IEvent<NamespaceDescriptionsWereUpdated> @event,
        NamespaceAggregate aggregate
    ) =>
        aggregate with
        {
            DisplayName = @event.Data.DisplayName,
            Description = @event.Data.Description,
            Documentation = @event.Data.Documentation,
            ModifiedAt = @event.Timestamp,
        };

    public static NamespaceAggregate Apply(
        IEvent<NamespaceWasSoftDeleted> @event,
        NamespaceAggregate aggregate
    ) => aggregate with { DeletedAt = @event.Timestamp };

    public static NamespaceAggregate Apply(
        IEvent<NamespaceWasRestored> @event,
        NamespaceAggregate aggregate
    ) => aggregate with { DeletedAt = null };

    public static NamespaceAggregate? Apply(
        IEvent<NamespaceWasPermanentlyDeleted> @event,
        NamespaceAggregate aggregate
    ) => null;
}

// Events

public abstract record NamespaceDomainEvent;

public sealed record NamespaceWasCreated(
    string? DisplayName,
    string? Description,
    string? Documentation
) : NamespaceDomainEvent;

public sealed record NamespaceDescriptionsWereUpdated(
    string? DisplayName,
    string? Description,
    string? Documentation
) : NamespaceDomainEvent;

public record NamespaceWasSoftDeleted : NamespaceDomainEvent;

public record NamespaceWasRestored : NamespaceDomainEvent;

public record NamespaceWasPermanentlyDeleted : NamespaceDomainEvent;
