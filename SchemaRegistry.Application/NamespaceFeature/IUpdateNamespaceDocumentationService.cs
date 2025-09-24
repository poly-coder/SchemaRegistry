using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.Application.NamespaceFeature;

public interface IUpdateNamespaceDocumentationService
{
    Task<NamespaceCommandResult> UpdateNamespaceDocumentationAsync(
        UpdateNamespaceDocumentationCommand command,
        CancellationToken cancel = default
    );
}
