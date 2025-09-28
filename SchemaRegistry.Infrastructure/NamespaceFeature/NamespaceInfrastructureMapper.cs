namespace SchemaRegistry.Infrastructure.NamespaceFeature;

internal static class NamespaceInfrastructureMapper
{
    public static NamespaceDetails MapToDetails(this NamespaceAggregate source) =>
        new(
            Name: source.Name,
            DisplayName: source.DisplayName,
            Description: source.Description,
            Documentation: source.Documentation,
            Status: source.Status,
            CreatedAt: source.CreatedAt,
            ModifiedAt: source.ModifiedAt,
            DeletedAt: source.DeletedAt,
            Version: source.Version
        );

    #region [ MapToDomain ]

    public static Domain.NamespaceFeature.NamespaceStatus MapToDomain(
        this NamespaceStatus source
    ) =>
        source switch
        {
            NamespaceStatus.Active => Domain.NamespaceFeature.NamespaceStatus.Active,
            NamespaceStatus.Deleted => Domain.NamespaceFeature.NamespaceStatus.Deleted,
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
        };

    public static Domain.NamespaceFeature.NamespaceDetails MapToDomain(
        this NamespaceDetails source
    ) =>
        new(
            Name: source.Name,
            DisplayName: source.DisplayName,
            Description: source.Description,
            Documentation: source.Documentation,
            Status: source.Status.MapToDomain(),
            CreatedAt: source.CreatedAt,
            ModifiedAt: source.ModifiedAt,
            DeletedAt: source.DeletedAt,
            Version: source.Version
        );

    public static Domain.NamespaceFeature.NamespaceOperations MapToDomain(
        this NamespaceOperations source
    ) =>
        new(
            CanUpdateDescriptions: source.CanUpdateDescriptions,
            CanUpdateDocumentation: source.CanUpdateDocumentation,
            CanDelete: source.CanDelete,
            CanRestore: source.CanRestore
        );

    public static Domain.NamespaceFeature.NamespaceCommandResult MapToDomain(
        this NamespaceCommandResult source
    ) => new(Updated: source.Updated);

    public static Domain.NamespaceFeature.GetNamespaceByIdQueryResult MapToDomain(
        this GetNamespaceByIdQueryResult source
    ) => new(Namespace: source.Namespace.MapToDomain());

    public static Domain.NamespaceFeature.NamespaceDetailsInfo MapToDomain(
        this NamespaceDetailsInfo source
    ) => new(Details: source.Details.MapToDomain(), Operations: source.Operations.MapToDomain());

    #endregion [ MapToDomain ]

    #region [ MapToOrleans ]

    public static CreateNamespaceCommand MapToOrleans(
        this Domain.NamespaceFeature.CreateNamespaceCommand source
    ) =>
        new(
            DisplayName: source.DisplayName,
            Description: source.Description,
            Documentation: source.Documentation
        );

    public static UpdateNamespaceDescriptionsCommand MapToOrleans(
        this Domain.NamespaceFeature.UpdateNamespaceDescriptionsCommand source
    ) => new(DisplayName: source.DisplayName, Description: source.Description);

    public static UpdateNamespaceDocumentationCommand MapToOrleans(
        this Domain.NamespaceFeature.UpdateNamespaceDocumentationCommand source
    ) => new(Documentation: source.Documentation);

    public static DeleteNamespaceCommand MapToOrleans(
        this Domain.NamespaceFeature.DeleteNamespaceCommand source
    ) => new();

    public static RestoreNamespaceCommand MapToOrleans(
        this Domain.NamespaceFeature.RestoreNamespaceCommand source
    ) => new();

    public static GetNamespaceByIdQuery MapToOrleans(
        this Domain.NamespaceFeature.GetNamespaceByIdQuery source
    ) => new(Deleted: source.Deleted);

    #endregion [ MapToOrleans ]
}
