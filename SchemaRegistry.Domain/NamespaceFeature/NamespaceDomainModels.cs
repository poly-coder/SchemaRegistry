namespace SchemaRegistry.Domain.NamespaceFeature;

public enum NamespaceStatus
{
    Active = 0,
    Deleted = 1,
}

public sealed record NamespaceListItem(
    string Name,
    string? DisplayName,
    DateTimeOffset CreatedAt,
    DateTimeOffset ModifiedAt,
    DateTimeOffset? DeletedAt,
    NamespaceStatus Status
);

public sealed record NamespaceDetails(
    string Name,
    string? DisplayName,
    string? Description,
    string? Documentation,
    DateTimeOffset CreatedAt,
    DateTimeOffset ModifiedAt,
    DateTimeOffset? DeletedAt,
    NamespaceStatus Status
);

public sealed record NamespaceOperations(
    bool CanUpdateDescriptions,
    bool CanDelete,
    bool CanRestore
);

public sealed record NamespaceDetailsInfo(NamespaceDetails Details, NamespaceOperations Operations);

public sealed record NamespaceListItemInfo(NamespaceListItem Item, NamespaceOperations Operations);

public sealed record NamespaceCommandResult(bool Updated);
