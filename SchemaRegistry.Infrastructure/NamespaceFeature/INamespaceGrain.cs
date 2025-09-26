namespace SchemaRegistry.Infrastructure.NamespaceFeature;

public interface INamespaceGrain : IGrainWithStringKey
{
    Task<NamespaceCommandResult> CreateNamespace(
        CreateNamespaceCommand command,
        CancellationToken cancel
    );

    Task<NamespaceCommandResult> UpdateNamespaceDescriptions(
        UpdateNamespaceDescriptionsCommand command,
        CancellationToken cancel
    );

    Task<NamespaceCommandResult> UpdateNamespaceDocumentation(
        UpdateNamespaceDocumentationCommand command,
        CancellationToken cancel
    );

    Task<NamespaceCommandResult> DeleteNamespace(
        DeleteNamespaceCommand command,
        CancellationToken cancel
    );

    Task<NamespaceCommandResult> RestoreNamespace(
        RestoreNamespaceCommand command,
        CancellationToken cancel
    );

    Task<GetNamespaceByIdQueryResult> GetNamespaceById(
        GetNamespaceByIdQuery query,
        CancellationToken cancel
    );
}
