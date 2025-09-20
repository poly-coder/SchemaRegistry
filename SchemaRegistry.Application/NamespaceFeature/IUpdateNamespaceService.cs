using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.Application.NamespaceFeature;

public interface IUpdateNamespaceDescriptionsService
{
    Task<UpdateNamespaceDescriptionsCommandResult> UpdateNamespaceDescriptionsAsync(
        UpdateNamespaceDescriptionsCommand command,
        CancellationToken cancel = default
    );
}
