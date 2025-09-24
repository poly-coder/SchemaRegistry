using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.Application.NamespaceFeature;

public interface IDeleteNamespaceService
{
    Task<NamespaceCommandResult> DeleteNamespaceAsync(
        DeleteNamespaceCommand command,
        CancellationToken cancel = default
    );
}
