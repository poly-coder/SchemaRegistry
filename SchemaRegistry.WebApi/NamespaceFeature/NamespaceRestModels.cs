using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.WebApi.NamespaceFeature;

// Models

public sealed record CreateNamespaceRequestBody(
    string? DisplayName = null,
    string? Description = null,
    string? Documentation = null
);

public sealed record UpdateNamespaceDescriptionsRequestBody(
    string? DisplayName = null,
    string? Description = null
);

public sealed record UpdateNamespaceDocumentationRequestBody(string? Documentation = null);

public sealed record NamespaceResponseBody(bool Updated);

// Mapper

public static class CreateNamespaceRestMapper
{
    public static CreateNamespaceCommand MapToCommand(
        this CreateNamespaceRequestBody body,
        string name
    )
    {
        return new CreateNamespaceCommand(
            name,
            body.DisplayName,
            body.Description,
            body.Documentation
        );
    }

    public static UpdateNamespaceDescriptionsCommand MapToCommand(
        this UpdateNamespaceDescriptionsRequestBody body,
        string name
    )
    {
        return new UpdateNamespaceDescriptionsCommand(name, body.DisplayName, body.Description);
    }

    public static UpdateNamespaceDocumentationCommand MapToCommand(
        this UpdateNamespaceDocumentationRequestBody body,
        string name
    )
    {
        return new UpdateNamespaceDocumentationCommand(name, body.Documentation);
    }

    public static NamespaceResponseBody MapToResponseBody(this NamespaceCommandResult result)
    {
        return new(Updated: result.Updated);
    }
}
