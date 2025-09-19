using JasperFx.Events;
using Marten.Schema;

namespace SchemaRegistry.Infrastructure.NamespaceFeature;

public record NamespaceAggregate(
    [property: Identity] string Name,
    string? DisplayName,
    string? Description,
    string? Documentation,
    DateTimeOffset CreatedAt,
    DateTimeOffset? DeletedAt = null
)
{
    public static NamespaceAggregate Create(IEvent<NamespaceWasCreated> @event) =>
        new(
            @event.Data.Name,
            @event.Data.DisplayName,
            @event.Data.Description,
            @event.Data.Documentation,
            @event.Timestamp
        );
}

// Events

public record NamespaceWasCreated(
    string Name,
    string? DisplayName,
    string? Description,
    string? Documentation
);
