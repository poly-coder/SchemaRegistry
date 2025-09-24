using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.Application.NamespaceFeature;

public interface IUpdateNamespaceDescriptionsService
{
    Task<NamespaceCommandResult> UpdateNamespaceDescriptionsAsync(
        UpdateNamespaceDescriptionsCommand command,
        CancellationToken cancel = default
    );
}
