using SchemaRegistry.Application.NamespaceFeature;
using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.Infrastructure.NamespaceFeature;

internal sealed class CreateNamespaceService : ICreateNamespaceService
{
    public async Task<CreateNamespaceCommandResult> CreateNamespaceAsync(
        CreateNamespaceCommand command,
        CancellationToken cancel = default
    )
    {
        await Task.CompletedTask;

        return new();
    }
}
