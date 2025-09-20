namespace SchemaRegistry.Infrastructure.NamespaceFeature;

public interface INamespaceGrain : IGrainWithStringKey
{
    Task<CreateNamespaceOutput> CreateNamespace(
        CreateNamespaceInput command,
        CancellationToken cancel
    );

    Task<UpdateNamespaceDescriptionsOutput> UpdateNamespaceDescriptions(
        UpdateNamespaceDescriptionsInput command,
        CancellationToken cancel
    );

    Task<DeleteNamespaceOutput> DeleteNamespace(
        DeleteNamespaceInput command,
        CancellationToken cancel
    );

    Task<RestoreNamespaceOutput> RestoreNamespace(
        RestoreNamespaceInput command,
        CancellationToken cancel
    );

    Task<GetNamespaceByIdOutput> GetNamespaceById(
        GetNamespaceByIdInput command,
        CancellationToken cancel
    );
}
