using SchemaRegistry.Domain;

namespace SchemaRegistry.Infrastructure.NamespaceFeature;

[GenerateSerializer]
[Alias($"{SchemaRegistryDomain.ProjectName}.{nameof(CreateNamespaceCommand)}")]
public sealed record CreateNamespaceCommand(
    string? DisplayName,
    string? Description,
    string? Documentation
);

[GenerateSerializer]
[Alias($"{SchemaRegistryDomain.ProjectName}.{nameof(UpdateNamespaceDescriptionsCommand)}")]
public sealed record UpdateNamespaceDescriptionsCommand(string? DisplayName, string? Description);

[GenerateSerializer]
[Alias($"{SchemaRegistryDomain.ProjectName}.{nameof(UpdateNamespaceDocumentationCommand)}")]
public sealed record UpdateNamespaceDocumentationCommand(string? Documentation);

[GenerateSerializer]
[Alias($"{SchemaRegistryDomain.ProjectName}.{nameof(DeleteNamespaceCommand)}")]
public sealed record DeleteNamespaceCommand;

[GenerateSerializer]
[Alias($"{SchemaRegistryDomain.ProjectName}.{nameof(RestoreNamespaceCommand)}")]
public sealed record RestoreNamespaceCommand;

[GenerateSerializer]
[Alias($"{SchemaRegistryDomain.ProjectName}.{nameof(NamespaceCommandResult)}")]
public sealed record NamespaceCommandResult(bool Updated);

[GenerateSerializer]
[Alias($"{SchemaRegistryDomain.ProjectName}.{nameof(GetNamespaceByIdQuery)}")]
public sealed record GetNamespaceByIdQuery(bool Deleted = false);

[GenerateSerializer]
[Alias($"{SchemaRegistryDomain.ProjectName}.{nameof(GetNamespaceByIdQueryResult)}")]
public sealed record GetNamespaceByIdQueryResult(NamespaceDetailsInfo Namespace);

public enum NamespaceStatus
{
    Active = 0,
    Deleted = 1,
}

[GenerateSerializer]
[Alias($"{SchemaRegistryDomain.ProjectName}.{nameof(NamespaceDetails)}")]
public sealed record NamespaceDetails(
    string Name,
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
    public bool IsDeleted => DeletedAt.HasValue;
}

[GenerateSerializer]
[Alias($"{SchemaRegistryDomain.ProjectName}.{nameof(NamespaceOperations)}")]
public sealed record NamespaceOperations(
    bool CanUpdateDescriptions,
    bool CanUpdateDocumentation,
    bool CanDelete,
    bool CanRestore
);

[GenerateSerializer]
[Alias($"{SchemaRegistryDomain.ProjectName}.{nameof(NamespaceDetailsInfo)}")]
public sealed record NamespaceDetailsInfo(NamespaceDetails Details, NamespaceOperations Operations);

// Mapper
