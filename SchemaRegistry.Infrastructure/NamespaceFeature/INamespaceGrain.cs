using SchemaRegistry.Domain;

namespace SchemaRegistry.Infrastructure.NamespaceFeature;

public interface INamespaceGrain : IGrainWithStringKey
{
    Task<CreateNamespaceOutput> CreateNamespace(
        CreateNamespaceInput command,
        CancellationToken cancel
    );
}

[GenerateSerializer]
[Alias($"{SchemaRegistryDomain.ProjectName}.{nameof(CreateNamespaceInput)}")]
public sealed record CreateNamespaceInput(
    string Name,
    string? DisplayName,
    string? Description,
    string? Documentation
);

[GenerateSerializer]
[Alias($"{SchemaRegistryDomain.ProjectName}.{nameof(CreateNamespaceOutput)}")]
public sealed record CreateNamespaceOutput;
