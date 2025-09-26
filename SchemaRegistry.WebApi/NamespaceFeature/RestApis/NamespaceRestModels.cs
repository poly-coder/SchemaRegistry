namespace SchemaRegistry.WebApi.NamespaceFeature.RestApis;

// Command Models

public sealed record CreateNamespaceCommand(
    string? DisplayName = null,
    string? Description = null,
    string? Documentation = null
);

public sealed record UpdateNamespaceDescriptionsCommand(
    string? DisplayName = null,
    string? Description = null
);

public sealed record UpdateNamespaceDocumentationCommand(string? Documentation = null);

public sealed record NamespaceCommandResult(bool Updated);

// Query Models

public sealed record GetNamespaceByIdResult(NamespaceDetailsInfo Namespace);

// Data Models

public sealed record NamespaceListItem(
    string Name,
    string? DisplayName,
    DateTimeOffset CreatedAt,
    DateTimeOffset ModifiedAt,
    DateTimeOffset? DeletedAt,
    SchemaRegistry.Domain.NamespaceFeature.NamespaceStatus Status
);

public sealed record NamespaceDetails(
    string Name,
    string? DisplayName,
    string? Description,
    string? Documentation,
    DateTimeOffset CreatedAt,
    DateTimeOffset ModifiedAt,
    DateTimeOffset? DeletedAt,
    SchemaRegistry.Domain.NamespaceFeature.NamespaceStatus Status
);

public sealed record NamespaceOperations(
    bool CanUpdateDescriptions,
    bool CanDelete,
    bool CanRestore
);

public sealed record NamespaceDetailsInfo(NamespaceDetails Details, NamespaceOperations Operations);

public sealed record NamespaceListItemInfo(NamespaceListItem Item, NamespaceOperations Operations);

// Mapper

public static class NamespaceRestMapper
{
    public static GetNamespaceByIdResult MapToRestApiResult(
        this SchemaRegistry.Domain.NamespaceFeature.GetNamespaceByIdQueryResult source
    )
    {
        return new GetNamespaceByIdResult(source.Namespace.MapToRestApiModel());
    }

    public static SchemaRegistry.Domain.NamespaceFeature.CreateNamespaceCommand MapToCommand(
        this CreateNamespaceCommand source,
        string name
    )
    {
        return new SchemaRegistry.Domain.NamespaceFeature.CreateNamespaceCommand(
            name,
            source.DisplayName,
            source.Description,
            source.Documentation
        );
    }

    public static SchemaRegistry.Domain.NamespaceFeature.UpdateNamespaceDescriptionsCommand MapToCommand(
        this UpdateNamespaceDescriptionsCommand source,
        string name
    )
    {
        return new SchemaRegistry.Domain.NamespaceFeature.UpdateNamespaceDescriptionsCommand(
            name,
            source.DisplayName,
            source.Description
        );
    }

    public static SchemaRegistry.Domain.NamespaceFeature.UpdateNamespaceDocumentationCommand MapToCommand(
        this UpdateNamespaceDocumentationCommand source,
        string name
    )
    {
        return new SchemaRegistry.Domain.NamespaceFeature.UpdateNamespaceDocumentationCommand(
            name,
            source.Documentation
        );
    }

    public static NamespaceCommandResult MapToRestApiResult(
        this SchemaRegistry.Domain.NamespaceFeature.NamespaceCommandResult source
    )
    {
        return new(Updated: source.Updated);
    }

    public static NamespaceListItem MapToRestApiModel(
        this SchemaRegistry.Domain.NamespaceFeature.NamespaceListItem source
    )
    {
        return new NamespaceListItem(
            Name: source.Name,
            DisplayName: source.DisplayName,
            CreatedAt: source.CreatedAt,
            ModifiedAt: source.ModifiedAt,
            DeletedAt: source.DeletedAt,
            Status: source.Status
        );
    }

    public static NamespaceDetails MapToRestApiModel(
        this SchemaRegistry.Domain.NamespaceFeature.NamespaceDetails source
    )
    {
        return new NamespaceDetails(
            Name: source.Name,
            DisplayName: source.DisplayName,
            Description: source.Description,
            Documentation: source.Documentation,
            CreatedAt: source.CreatedAt,
            ModifiedAt: source.ModifiedAt,
            DeletedAt: source.DeletedAt,
            Status: source.Status
        );
    }

    public static NamespaceOperations MapToRestApiModel(
        this SchemaRegistry.Domain.NamespaceFeature.NamespaceOperations source
    )
    {
        return new NamespaceOperations(
            CanUpdateDescriptions: source.CanUpdateDescriptions,
            CanDelete: source.CanDelete,
            CanRestore: source.CanRestore
        );
    }

    public static NamespaceListItemInfo MapToRestApiModel(
        this SchemaRegistry.Domain.NamespaceFeature.NamespaceListItemInfo source
    )
    {
        return new NamespaceListItemInfo(
            source.Item.MapToRestApiModel(),
            source.Operations.MapToRestApiModel()
        );
    }

    public static NamespaceDetailsInfo MapToRestApiModel(
        this SchemaRegistry.Domain.NamespaceFeature.NamespaceDetailsInfo source
    )
    {
        return new NamespaceDetailsInfo(
            source.Details.MapToRestApiModel(),
            source.Operations.MapToRestApiModel()
        );
    }
}
