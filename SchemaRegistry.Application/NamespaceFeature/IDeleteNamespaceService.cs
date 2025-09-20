using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.Application.NamespaceFeature;

public interface IDeleteNamespaceService
{
    Task<DeleteNamespaceCommandResult> DeleteNamespaceAsync(
        DeleteNamespaceCommand command,
        CancellationToken cancel = default
    );
}
