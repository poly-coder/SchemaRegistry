using JasperFx.Events;
using Marten.Schema;
using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.Infrastructure.NamespaceFeature;

public sealed record NamespaceAggregate(
    [property: Identity] string Name,
    string? DisplayName,
    string? Description,
    string? Documentation,
    NamespaceStatus Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset ModifiedAt,
    DateTimeOffset? DeletedAt
)
{
    public static NamespaceAggregate Create(IEvent<NamespaceWasCreated> @event) =>
        new(
            Name: @event.StreamKey!,
            DisplayName: @event.Data.DisplayName,
            Description: @event.Data.Description,
            Documentation: @event.Data.Documentation,
            Status: NamespaceStatus.Active,
            CreatedAt: @event.Timestamp,
            ModifiedAt: @event.Timestamp,
            DeletedAt: null
        );

    public static NamespaceAggregate Apply(
        IEvent<NamespaceDescriptionsWereUpdated> @event,
        NamespaceAggregate aggregate
    ) =>
        aggregate with
        {
            DisplayName = @event.Data.DisplayName,
            Description = @event.Data.Description,
            ModifiedAt = @event.Timestamp,
        };

    public static NamespaceAggregate Apply(
        IEvent<NamespaceDocumentationWasUpdated> @event,
        NamespaceAggregate aggregate
    ) =>
        aggregate with
        {
            Documentation = @event.Data.Documentation,
            ModifiedAt = @event.Timestamp,
        };

    public static NamespaceAggregate Apply(
        IEvent<NamespaceWasDeleted> @event,
        NamespaceAggregate aggregate
    ) => aggregate with { DeletedAt = @event.Timestamp };

    public static NamespaceAggregate Apply(
        IEvent<NamespaceWasRestored> @event,
        NamespaceAggregate aggregate
    ) => aggregate with { DeletedAt = null };
}

// Events

public abstract record NamespaceDomainEvent;

public sealed record NamespaceWasCreated(
    string? DisplayName,
    string? Description,
    string? Documentation
) : NamespaceDomainEvent;

public sealed record NamespaceDescriptionsWereUpdated(string? DisplayName, string? Description)
    : NamespaceDomainEvent;

public sealed record NamespaceDocumentationWasUpdated(string? Documentation) : NamespaceDomainEvent;

public record NamespaceWasDeleted : NamespaceDomainEvent;

public record NamespaceWasRestored : NamespaceDomainEvent;
