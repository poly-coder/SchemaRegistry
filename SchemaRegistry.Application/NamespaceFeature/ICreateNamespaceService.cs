using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.Application.NamespaceFeature;

public interface ICreateNamespaceService
{
    Task<NamespaceCommandResult> CreateNamespaceAsync(
        CreateNamespaceCommand command,
        CancellationToken cancel = default
    );
}
