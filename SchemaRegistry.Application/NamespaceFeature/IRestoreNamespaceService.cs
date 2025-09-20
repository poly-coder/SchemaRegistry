using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.Application.NamespaceFeature;

public interface IRestoreNamespaceService
{
    Task<RestoreNamespaceCommandResult> RestoreNamespaceAsync(
        RestoreNamespaceCommand command,
        CancellationToken cancel = default
    );
}
