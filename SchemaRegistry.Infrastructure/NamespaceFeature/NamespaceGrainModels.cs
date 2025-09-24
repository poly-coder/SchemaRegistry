using SchemaRegistry.Domain;
using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.Infrastructure.NamespaceFeature;

[GenerateSerializer]
[Alias($"{SchemaRegistryDomain.ProjectName}.{nameof(CreateNamespaceInput)}")]
public sealed record CreateNamespaceInput(
    string? DisplayName,
    string? Description,
    string? Documentation
);

[GenerateSerializer]
[Alias($"{SchemaRegistryDomain.ProjectName}.{nameof(UpdateNamespaceDescriptionsInput)}")]
public sealed record UpdateNamespaceDescriptionsInput(string? DisplayName, string? Description);

[GenerateSerializer]
[Alias($"{SchemaRegistryDomain.ProjectName}.{nameof(UpdateNamespaceDocumentationInput)}")]
public sealed record UpdateNamespaceDocumentationInput(string? Documentation);

[GenerateSerializer]
[Alias($"{SchemaRegistryDomain.ProjectName}.{nameof(DeleteNamespaceInput)}")]
public sealed record DeleteNamespaceInput;

[GenerateSerializer]
[Alias($"{SchemaRegistryDomain.ProjectName}.{nameof(RestoreNamespaceInput)}")]
public sealed record RestoreNamespaceInput;

[GenerateSerializer]
[Alias($"{SchemaRegistryDomain.ProjectName}.{nameof(NamespaceCommandOutput)}")]
public sealed record NamespaceCommandOutput(bool Updated);

[GenerateSerializer]
[Alias($"{SchemaRegistryDomain.ProjectName}.{nameof(GetNamespaceByIdInput)}")]
public sealed record GetNamespaceByIdInput(bool Deleted);

[GenerateSerializer]
[Alias($"{SchemaRegistryDomain.ProjectName}.{nameof(GetNamespaceByIdOutput)}")]
public sealed record GetNamespaceByIdOutput(
    NamespaceDetailsData Details,
    NamespaceOperationsData Operations
);

[GenerateSerializer]
[Alias($"{SchemaRegistryDomain.ProjectName}.{nameof(NamespaceDetailsData)}")]
public sealed record NamespaceDetailsData(
    string Name,
    string? DisplayName,
    string? Description,
    string? Documentation,
    NamespaceStatus Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset ModifiedAt,
    DateTimeOffset? DeletedAt
)
{
    public bool IsDeleted => DeletedAt.HasValue;
}

[GenerateSerializer]
[Alias($"{SchemaRegistryDomain.ProjectName}.{nameof(NamespaceOperationsData)}")]
public sealed record NamespaceOperationsData(
    bool CanUpdateDescriptions,
    bool CanDelete,
    bool CanRestore
);

// Mapper

public static class NamespaceGrainModelsMapper
{
    public static NamespaceDetailsData MapToDetails(this NamespaceAggregate aggregate) =>
        new(
            Name: aggregate.Name,
            DisplayName: aggregate.DisplayName,
            Description: aggregate.Description,
            Documentation: aggregate.Documentation,
            Status: aggregate.Status,
            CreatedAt: aggregate.CreatedAt,
            ModifiedAt: aggregate.ModifiedAt,
            DeletedAt: aggregate.DeletedAt
        );

    public static NamespaceOperationsData MapToOperations(this NamespaceAggregate aggregate) =>
        new(
            CanDelete: aggregate.Status != NamespaceStatus.Deleted,
            CanRestore: aggregate.Status != NamespaceStatus.Active,
            CanUpdateDescriptions: aggregate.Status == NamespaceStatus.Active
        );

    public static NamespaceDetails MapToDomain(this NamespaceDetailsData data) =>
        new(
            Name: data.Name,
            DisplayName: data.DisplayName,
            Description: data.Description,
            Documentation: data.Documentation,
            Status: data.Status,
            CreatedAt: data.CreatedAt,
            ModifiedAt: data.ModifiedAt,
            DeletedAt: data.DeletedAt
        );

    public static NamespaceOperations MapToDomain(this NamespaceOperationsData data) =>
        new(
            CanUpdateDescriptions: data.CanUpdateDescriptions,
            CanDelete: data.CanDelete,
            CanRestore: data.CanRestore
        );
}
