using SchemaRegistry.Domain.NamespaceFeature;

namespace SchemaRegistry.WebApi.NamespaceFeature;

// Models

public sealed record CreateNamespaceRequestBody(
    string? DisplayName = null,
    string? Description = null,
    string? Documentation = null
);

public sealed record CreateNamespaceResponseBody();

public sealed record UpdateNamespaceDescriptionsRequestBody(
    string? DisplayName = null,
    string? Description = null,
    string? Documentation = null
);

public sealed record UpdateNamespaceDescriptionsResponseBody();

public sealed record DeleteNamespaceResponseBody(bool PermanentlyDeleted);

public sealed record RestoreNamespaceResponseBody();

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

    public static CreateNamespaceResponseBody MapToResponseBody(
        this CreateNamespaceCommandResult result
    )
    {
        return new();
    }

    public static UpdateNamespaceDescriptionsCommand MapToCommand(
        this UpdateNamespaceDescriptionsRequestBody body,
        string name
    )
    {
        return new UpdateNamespaceDescriptionsCommand(
            name,
            body.DisplayName,
            body.Description,
            body.Documentation
        );
    }

    public static UpdateNamespaceDescriptionsResponseBody MapToResponseBody(
        this UpdateNamespaceDescriptionsCommandResult result
    )
    {
        return new();
    }

    public static DeleteNamespaceResponseBody MapToResponseBody(
        this DeleteNamespaceCommandResult result
    )
    {
        return new(result.PermanentlyDeleted);
    }

    public static RestoreNamespaceResponseBody MapToResponseBody(
        this RestoreNamespaceCommandResult result
    )
    {
        return new();
    }
}
