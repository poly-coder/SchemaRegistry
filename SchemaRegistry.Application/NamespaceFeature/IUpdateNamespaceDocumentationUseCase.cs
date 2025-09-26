using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.Application.NamespaceFeature;

public interface IUpdateNamespaceDocumentationUseCase
{
    Task<NamespaceCommandResult> UpdateNamespaceDocumentationAsync(
        UpdateNamespaceDocumentationCommand command,
        CancellationToken cancel = default
    );
}
