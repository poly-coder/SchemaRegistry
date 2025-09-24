namespace SchemaRegistry.Infrastructure.NamespaceFeature;

public interface INamespaceGrain : IGrainWithStringKey
{
    Task<NamespaceCommandOutput> CreateNamespace(
        CreateNamespaceInput command,
        CancellationToken cancel
    );

    Task<NamespaceCommandOutput> UpdateNamespaceDescriptions(
        UpdateNamespaceDescriptionsInput command,
        CancellationToken cancel
    );

    Task<NamespaceCommandOutput> UpdateNamespaceDocumentation(
        UpdateNamespaceDocumentationInput command,
        CancellationToken cancel
    );

    Task<NamespaceCommandOutput> DeleteNamespace(
        DeleteNamespaceInput command,
        CancellationToken cancel
    );

    Task<NamespaceCommandOutput> RestoreNamespace(
        RestoreNamespaceInput command,
        CancellationToken cancel
    );

    Task<GetNamespaceByIdOutput> GetNamespaceById(
        GetNamespaceByIdInput command,
        CancellationToken cancel
    );
}
