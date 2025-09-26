using JasperFx.Events;
using Marten.Schema;

namespace SchemaRegistry.Infrastructure.NamespaceFeature;

public sealed record NamespaceAggregate(
    [property: Identity] string Name,
    string? DisplayName,
    string? Description,
    string? Documentation,
    NamespaceStatus Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset ModifiedAt,
    DateTimeOffset? DeletedAt,
    long Version
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
            DeletedAt: null,
            Version: @event.Version
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
            Version = @event.Version,
        };

    public static NamespaceAggregate Apply(
        IEvent<NamespaceDocumentationWasUpdated> @event,
        NamespaceAggregate aggregate
    ) =>
        aggregate with
        {
            Documentation = @event.Data.Documentation,
            ModifiedAt = @event.Timestamp,
            Version = @event.Version,
        };

    public static NamespaceAggregate Apply(
        IEvent<NamespaceWasDeleted> @event,
        NamespaceAggregate aggregate
    ) =>
        aggregate with
        {
            Status = NamespaceStatus.Deleted,
            DeletedAt = @event.Timestamp,
            ModifiedAt = @event.Timestamp,
            Version = @event.Version,
        };

    public static NamespaceAggregate Apply(
        IEvent<NamespaceWasRestored> @event,
        NamespaceAggregate aggregate
    ) =>
        aggregate with
        {
            Status = NamespaceStatus.Active,
            DeletedAt = null,
            ModifiedAt = @event.Timestamp,
            Version = @event.Version,
        };
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

// Commands


public static class NamespaceAggregateExtensions
{
    public static IEnumerable<NamespaceDomainEvent> GetEvents(this CreateNamespaceCommand command)
    {
        yield return new NamespaceWasCreated(
            DisplayName: command.DisplayName,
            Description: command.Description,
            Documentation: command.Documentation
        );
    }

    public static IEnumerable<NamespaceDomainEvent> GetEvents(
        this UpdateNamespaceDescriptionsCommand command,
        NamespaceAggregate aggregate
    )
    {
        if (
            StringComparer.Ordinal.Equals(command.DisplayName, aggregate.DisplayName)
            && StringComparer.Ordinal.Equals(command.Description, aggregate.Description)
        )
        {
            yield break;
        }

        yield return new NamespaceDescriptionsWereUpdated(
            DisplayName: command.DisplayName,
            Description: command.Description
        );
    }

    public static IEnumerable<NamespaceDomainEvent> GetEvents(
        this UpdateNamespaceDocumentationCommand command,
        NamespaceAggregate aggregate
    )
    {
        if (StringComparer.Ordinal.Equals(command.Documentation, aggregate.Documentation))
        {
            yield break;
        }

        yield return new NamespaceDocumentationWasUpdated(Documentation: command.Documentation);
    }

    public static IEnumerable<NamespaceDomainEvent> GetEvents(
        this DeleteNamespaceCommand command,
        NamespaceAggregate aggregate
    )
    {
        if (aggregate.Status is NamespaceStatus.Deleted)
        {
            yield break;
        }

        yield return new NamespaceWasDeleted();
    }

    public static IEnumerable<NamespaceDomainEvent> GetEvents(
        this RestoreNamespaceCommand command,
        NamespaceAggregate aggregate
    )
    {
        if (aggregate.Status is NamespaceStatus.Active)
        {
            yield break;
        }

        yield return new NamespaceWasRestored();
    }

    public static NamespaceDetailsInfo GetDetailsInfo(
        this GetNamespaceByIdQuery query,
        NamespaceAggregate aggregate
    )
    {
        var details = aggregate.MapToDetails();

        var canUpdate = aggregate.Status == NamespaceStatus.Active;

        var operations = new NamespaceOperations(
            CanUpdateDescriptions: canUpdate,
            CanUpdateDocumentation: canUpdate,
            CanDelete: aggregate.Status != NamespaceStatus.Deleted,
            CanRestore: aggregate.Status != NamespaceStatus.Active
        );

        return new NamespaceDetailsInfo(details, operations);
    }
}
