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
[Alias($"{SchemaRegistryDomain.ProjectName}.{nameof(CreateNamespaceOutput)}")]
public sealed record CreateNamespaceOutput;

[GenerateSerializer]
[Alias($"{SchemaRegistryDomain.ProjectName}.{nameof(UpdateNamespaceDescriptionsInput)}")]
public sealed record UpdateNamespaceDescriptionsInput(
    string? DisplayName,
    string? Description,
    string? Documentation
);

[GenerateSerializer]
[Alias($"{SchemaRegistryDomain.ProjectName}.{nameof(UpdateNamespaceDescriptionsOutput)}")]
public sealed record UpdateNamespaceDescriptionsOutput;

[GenerateSerializer]
[Alias($"{SchemaRegistryDomain.ProjectName}.{nameof(DeleteNamespaceInput)}")]
public sealed record DeleteNamespaceInput(bool Permanently);

[GenerateSerializer]
[Alias($"{SchemaRegistryDomain.ProjectName}.{nameof(DeleteNamespaceOutput)}")]
public sealed record DeleteNamespaceOutput(bool PermanentlyDeleted);

[GenerateSerializer]
[Alias($"{SchemaRegistryDomain.ProjectName}.{nameof(RestoreNamespaceInput)}")]
public sealed record RestoreNamespaceInput;

[GenerateSerializer]
[Alias($"{SchemaRegistryDomain.ProjectName}.{nameof(RestoreNamespaceOutput)}")]
public sealed record RestoreNamespaceOutput;

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
    DateTimeOffset CreatedAt,
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
    bool CanRestore,
    bool CanDeletePermanently
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
            CreatedAt: aggregate.CreatedAt,
            DeletedAt: aggregate.DeletedAt
        );

    public static NamespaceDetails MapToDomain(this NamespaceDetailsData data) =>
        new(
            Name: data.Name,
            DisplayName: data.DisplayName,
            Description: data.Description,
            Documentation: data.Documentation,
            CreatedAt: data.CreatedAt,
            DeletedAt: data.DeletedAt,
            IsDeleted: data.IsDeleted
        );

    public static NamespaceOperations MapToDomain(this NamespaceOperationsData data) =>
        new(
            CanUpdateDescriptions: data.CanUpdateDescriptions,
            CanDelete: data.CanDelete,
            CanRestore: data.CanRestore,
            CanDeletePermanently: data.CanDeletePermanently
        );
}
