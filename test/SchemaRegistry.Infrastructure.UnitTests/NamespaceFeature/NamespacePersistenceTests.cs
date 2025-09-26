using JasperFx.Events;
using Pico.Testing;
using SchemaRegistry.Infrastructure.NamespaceFeature;

namespace SchemaRegistry.Infrastructure.UnitTests.NamespaceFeature;

[UnitTest]
public class NamespaceAggregateTests
{
    // Create

    [Theory]
    [MemberData(nameof(GetCreateTestData))]
    public void CreateTest(CreateTestData test)
    {
        var actual = NamespaceAggregate.Create(test.Event);

        actual.ShouldBe(test.Expected);
    }

    public sealed record CreateTestData(
        Event<NamespaceWasCreated> Event,
        NamespaceAggregate Expected
    );

    public static TheoryData<CreateTestData> GetCreateTestData() =>
        [
            new(
                new Event<NamespaceWasCreated>(
                    new NamespaceWasCreated(
                        DisplayName: "My namespace",
                        Description: "Some description",
                        Documentation: "# Namespace Documentation"
                    )
                )
                {
                    StreamKey = "my-namespace",
                    Timestamp = new DateTimeOffset(2025, 9, 25, 0, 0, 0, TimeSpan.Zero),
                    Version = 1L,
                },
                new NamespaceAggregate(
                    Name: "my-namespace",
                    DisplayName: "My namespace",
                    Description: "Some description",
                    Documentation: "# Namespace Documentation",
                    Status: NamespaceStatus.Active,
                    CreatedAt: new DateTimeOffset(2025, 9, 25, 0, 0, 0, TimeSpan.Zero),
                    ModifiedAt: new DateTimeOffset(2025, 9, 25, 0, 0, 0, TimeSpan.Zero),
                    DeletedAt: null,
                    Version: 1L
                )
            ),
        ];

    // Apply Descriptions Updated

    [Theory]
    [MemberData(nameof(GetApplyDescriptionsTestData))]
    public void ApplyDescriptionsTest(ApplyDescriptionsTestData test)
    {
        var actual = NamespaceAggregate.Apply(test.Event, test.Aggregate);

        actual.ShouldBe(test.Expected);
    }

    public sealed record ApplyDescriptionsTestData(
        Event<NamespaceDescriptionsWereUpdated> Event,
        NamespaceAggregate Aggregate,
        NamespaceAggregate Expected
    );

    public static TheoryData<ApplyDescriptionsTestData> GetApplyDescriptionsTestData()
    {
        var aggregate = new NamespaceAggregate(
            Name: "my-namespace",
            DisplayName: "My namespace",
            Description: "Some description",
            Documentation: "# Namespace Documentation",
            Status: NamespaceStatus.Active,
            CreatedAt: new DateTimeOffset(2025, 9, 25, 0, 0, 0, TimeSpan.Zero),
            ModifiedAt: new DateTimeOffset(2025, 9, 25, 0, 0, 0, TimeSpan.Zero),
            DeletedAt: null,
            Version: 1L
        );

        return
        [
            new(
                new Event<NamespaceDescriptionsWereUpdated>(
                    new NamespaceDescriptionsWereUpdated(
                        DisplayName: "New namespace",
                        Description: "New description"
                    )
                )
                {
                    StreamKey = "my-namespace",
                    Timestamp = new DateTimeOffset(2025, 9, 26, 0, 0, 0, TimeSpan.Zero),
                    Version = 2L,
                },
                aggregate,
                aggregate with
                {
                    DisplayName = "New namespace",
                    Description = "New description",
                    ModifiedAt = new DateTimeOffset(2025, 9, 26, 0, 0, 0, TimeSpan.Zero),
                    Version = 2L,
                }
            ),
        ];
    }

    // Apply Documentation Updated

    [Theory]
    [MemberData(nameof(GetApplyDocumentationTestData))]
    public void ApplyDocumentationTest(ApplyDocumentationTestData test)
    {
        var actual = NamespaceAggregate.Apply(test.Event, test.Aggregate);

        actual.ShouldBe(test.Expected);
    }

    public sealed record ApplyDocumentationTestData(
        Event<NamespaceDocumentationWasUpdated> Event,
        NamespaceAggregate Aggregate,
        NamespaceAggregate Expected
    );

    public static TheoryData<ApplyDocumentationTestData> GetApplyDocumentationTestData()
    {
        var aggregate = new NamespaceAggregate(
            Name: "my-namespace",
            DisplayName: "My namespace",
            Description: "Some description",
            Documentation: "# Namespace Documentation",
            Status: NamespaceStatus.Active,
            CreatedAt: new DateTimeOffset(2025, 9, 25, 0, 0, 0, TimeSpan.Zero),
            ModifiedAt: new DateTimeOffset(2025, 9, 25, 0, 0, 0, TimeSpan.Zero),
            DeletedAt: null,
            Version: 1L
        );

        return
        [
            new(
                new Event<NamespaceDocumentationWasUpdated>(
                    new NamespaceDocumentationWasUpdated(Documentation: "# New Documentation")
                )
                {
                    StreamKey = "my-namespace",
                    Timestamp = new DateTimeOffset(2025, 9, 26, 0, 0, 0, TimeSpan.Zero),
                    Version = 2L,
                },
                aggregate,
                aggregate with
                {
                    Documentation = "# New Documentation",
                    ModifiedAt = new DateTimeOffset(2025, 9, 26, 0, 0, 0, TimeSpan.Zero),
                    Version = 2L,
                }
            ),
        ];
    }

    // Apply Delete

    [Theory]
    [MemberData(nameof(GetApplyDeleteTestData))]
    public void ApplyDeleteTest(ApplyDeleteTestData test)
    {
        var actual = NamespaceAggregate.Apply(test.Event, test.Aggregate);

        actual.ShouldBe(test.Expected);
    }

    public sealed record ApplyDeleteTestData(
        Event<NamespaceWasDeleted> Event,
        NamespaceAggregate Aggregate,
        NamespaceAggregate Expected
    );

    public static TheoryData<ApplyDeleteTestData> GetApplyDeleteTestData()
    {
        var aggregate = new NamespaceAggregate(
            Name: "my-namespace",
            DisplayName: "My namespace",
            Description: "Some description",
            Documentation: "# Namespace Documentation",
            Status: NamespaceStatus.Active,
            CreatedAt: new DateTimeOffset(2025, 9, 25, 0, 0, 0, TimeSpan.Zero),
            ModifiedAt: new DateTimeOffset(2025, 9, 25, 0, 0, 0, TimeSpan.Zero),
            DeletedAt: null,
            Version: 1L
        );

        return
        [
            new(
                new Event<NamespaceWasDeleted>(new NamespaceWasDeleted())
                {
                    StreamKey = "my-namespace",
                    Timestamp = new DateTimeOffset(2025, 9, 26, 0, 0, 0, TimeSpan.Zero),
                    Version = 2L,
                },
                aggregate,
                aggregate with
                {
                    Status = NamespaceStatus.Deleted,
                    ModifiedAt = new DateTimeOffset(2025, 9, 26, 0, 0, 0, TimeSpan.Zero),
                    DeletedAt = new DateTimeOffset(2025, 9, 26, 0, 0, 0, TimeSpan.Zero),
                    Version = 2L,
                }
            ),
        ];
    }

    // Apply Restore

    [Theory]
    [MemberData(nameof(GetApplyRestoreTestData))]
    public void ApplyRestoreTest(ApplyRestoreTestData test)
    {
        var actual = NamespaceAggregate.Apply(test.Event, test.Aggregate);

        actual.ShouldBe(test.Expected);
    }

    public sealed record ApplyRestoreTestData(
        Event<NamespaceWasRestored> Event,
        NamespaceAggregate Aggregate,
        NamespaceAggregate Expected
    );

    public static TheoryData<ApplyRestoreTestData> GetApplyRestoreTestData()
    {
        var aggregate = new NamespaceAggregate(
            Name: "my-namespace",
            DisplayName: "My namespace",
            Description: "Some description",
            Documentation: "# Namespace Documentation",
            Status: NamespaceStatus.Active,
            CreatedAt: new DateTimeOffset(2025, 9, 25, 0, 0, 0, TimeSpan.Zero),
            ModifiedAt: new DateTimeOffset(2025, 9, 25, 0, 0, 0, TimeSpan.Zero),
            DeletedAt: new DateTimeOffset(2025, 9, 25, 0, 0, 0, TimeSpan.Zero),
            Version: 1L
        );

        return
        [
            new(
                new Event<NamespaceWasRestored>(new NamespaceWasRestored())
                {
                    StreamKey = "my-namespace",
                    Timestamp = new DateTimeOffset(2025, 9, 26, 0, 0, 0, TimeSpan.Zero),
                    Version = 2L,
                },
                aggregate,
                aggregate with
                {
                    Status = NamespaceStatus.Active,
                    ModifiedAt = new DateTimeOffset(2025, 9, 26, 0, 0, 0, TimeSpan.Zero),
                    DeletedAt = null,
                    Version = 2L,
                }
            ),
        ];
    }
}

[UnitTest]
public class NamespaceAggregateExtensionsTests
{
    // Create

    [Theory]
    [MemberData(nameof(GetCreateTestData))]
    public void CreateTest(CreateTestData test)
    {
        var actual = test.Command.GetEvents().ToArray();

        actual.ShouldBe(test.ExpectedEvents);
    }

    public sealed record CreateTestData(
        CreateNamespaceCommand Command,
        NamespaceDomainEvent[] ExpectedEvents
    );

    public static TheoryData<CreateTestData> GetCreateTestData() =>
        [
            new(
                new CreateNamespaceCommand(
                    DisplayName: "My namespace",
                    Description: "Some description",
                    Documentation: "# Namespace Documentation"
                ),
                [
                    new NamespaceWasCreated(
                        DisplayName: "My namespace",
                        Description: "Some description",
                        Documentation: "# Namespace Documentation"
                    ),
                ]
            ),
        ];

    // UpdateDescriptions

    [Theory]
    [MemberData(nameof(GetUpdateDescriptionsTestData))]
    public void UpdateDescriptionsTest(UpdateDescriptionsTestData test)
    {
        var actual = test.Command.GetEvents(test.Aggregate).ToArray();

        actual.ShouldBe(test.ExpectedEvents);
    }

    public sealed record UpdateDescriptionsTestData(
        UpdateNamespaceDescriptionsCommand Command,
        NamespaceAggregate Aggregate,
        NamespaceDomainEvent[] ExpectedEvents
    );

    public static TheoryData<UpdateDescriptionsTestData> GetUpdateDescriptionsTestData() =>
        [
            new(
                new UpdateNamespaceDescriptionsCommand(
                    DisplayName: "My namespace",
                    Description: "Some description"
                ),
                new NamespaceAggregate(
                    Name: "my-namespace",
                    DisplayName: "My namespace",
                    Description: "Some description",
                    Documentation: "# Namespace Documentation",
                    Status: NamespaceStatus.Active,
                    CreatedAt: new DateTimeOffset(2025, 9, 25, 0, 0, 0, TimeSpan.Zero),
                    ModifiedAt: new DateTimeOffset(2025, 9, 26, 0, 0, 0, TimeSpan.Zero),
                    DeletedAt: null,
                    Version: 10
                ),
                []
            ),
            new(
                new UpdateNamespaceDescriptionsCommand(
                    DisplayName: "My namespace",
                    Description: "Some description"
                ),
                new NamespaceAggregate(
                    Name: "my-namespace",
                    DisplayName: "Old namespace",
                    Description: "Some old description",
                    Documentation: "# Namespace old Documentation",
                    Status: NamespaceStatus.Active,
                    CreatedAt: new DateTimeOffset(2025, 9, 25, 0, 0, 0, TimeSpan.Zero),
                    ModifiedAt: new DateTimeOffset(2025, 9, 26, 0, 0, 0, TimeSpan.Zero),
                    DeletedAt: null,
                    Version: 10
                ),
                [
                    new NamespaceDescriptionsWereUpdated(
                        DisplayName: "My namespace",
                        Description: "Some description"
                    ),
                ]
            ),
        ];

    // UpdateDocumentation

    [Theory]
    [MemberData(nameof(GetUpdateDocumentationTestData))]
    public void UpdateDocumentationTest(UpdateDocumentationTestData test)
    {
        var actual = test.Command.GetEvents(test.Aggregate).ToArray();

        actual.ShouldBe(test.ExpectedEvents);
    }

    public sealed record UpdateDocumentationTestData(
        UpdateNamespaceDocumentationCommand Command,
        NamespaceAggregate Aggregate,
        NamespaceDomainEvent[] ExpectedEvents
    );

    public static TheoryData<UpdateDocumentationTestData> GetUpdateDocumentationTestData() =>
        [
            new(
                new UpdateNamespaceDocumentationCommand(Documentation: "# Namespace Documentation"),
                new NamespaceAggregate(
                    Name: "my-namespace",
                    DisplayName: "My namespace",
                    Description: "Some description",
                    Documentation: "# Namespace Documentation",
                    Status: NamespaceStatus.Active,
                    CreatedAt: new DateTimeOffset(2025, 9, 25, 0, 0, 0, TimeSpan.Zero),
                    ModifiedAt: new DateTimeOffset(2025, 9, 26, 0, 0, 0, TimeSpan.Zero),
                    DeletedAt: null,
                    Version: 10
                ),
                []
            ),
            new(
                new UpdateNamespaceDocumentationCommand(Documentation: "# Namespace Documentation"),
                new NamespaceAggregate(
                    Name: "my-namespace",
                    DisplayName: "Old namespace",
                    Description: "Some old description",
                    Documentation: "# Namespace old Documentation",
                    Status: NamespaceStatus.Active,
                    CreatedAt: new DateTimeOffset(2025, 9, 25, 0, 0, 0, TimeSpan.Zero),
                    ModifiedAt: new DateTimeOffset(2025, 9, 26, 0, 0, 0, TimeSpan.Zero),
                    DeletedAt: null,
                    Version: 10
                ),
                [new NamespaceDocumentationWasUpdated(Documentation: "# Namespace Documentation")]
            ),
        ];

    // Delete

    [Theory]
    [MemberData(nameof(GetDeleteTestData))]
    public void DeleteTest(DeleteTestData test)
    {
        var actual = test.Command.GetEvents(test.Aggregate).ToArray();

        actual.ShouldBe(test.ExpectedEvents);
    }

    public sealed record DeleteTestData(
        DeleteNamespaceCommand Command,
        NamespaceAggregate Aggregate,
        NamespaceDomainEvent[] ExpectedEvents
    );

    public static TheoryData<DeleteTestData> GetDeleteTestData() =>
        [
            new(
                new DeleteNamespaceCommand(),
                new NamespaceAggregate(
                    Name: "my-namespace",
                    DisplayName: "My namespace",
                    Description: "Some description",
                    Documentation: "# Namespace Documentation",
                    Status: NamespaceStatus.Deleted,
                    CreatedAt: new DateTimeOffset(2025, 9, 25, 0, 0, 0, TimeSpan.Zero),
                    ModifiedAt: new DateTimeOffset(2025, 9, 26, 0, 0, 0, TimeSpan.Zero),
                    DeletedAt: new DateTimeOffset(2025, 9, 26, 0, 0, 0, TimeSpan.Zero),
                    Version: 10
                ),
                []
            ),
            new(
                new DeleteNamespaceCommand(),
                new NamespaceAggregate(
                    Name: "my-namespace",
                    DisplayName: "Old namespace",
                    Description: "Some old description",
                    Documentation: "# Namespace old Documentation",
                    Status: NamespaceStatus.Active,
                    CreatedAt: new DateTimeOffset(2025, 9, 25, 0, 0, 0, TimeSpan.Zero),
                    ModifiedAt: new DateTimeOffset(2025, 9, 26, 0, 0, 0, TimeSpan.Zero),
                    DeletedAt: null,
                    Version: 10
                ),
                [new NamespaceWasDeleted()]
            ),
        ];

    // Restore

    [Theory]
    [MemberData(nameof(GetRestoreTestData))]
    public void RestoreTest(RestoreTestData test)
    {
        var actual = test.Command.GetEvents(test.Aggregate).ToArray();

        actual.ShouldBe(test.ExpectedEvents);
    }

    public sealed record RestoreTestData(
        RestoreNamespaceCommand Command,
        NamespaceAggregate Aggregate,
        NamespaceDomainEvent[] ExpectedEvents
    );

    public static TheoryData<RestoreTestData> GetRestoreTestData() =>
        [
            new(
                new RestoreNamespaceCommand(),
                new NamespaceAggregate(
                    Name: "my-namespace",
                    DisplayName: "My namespace",
                    Description: "Some description",
                    Documentation: "# Namespace Documentation",
                    Status: NamespaceStatus.Deleted,
                    CreatedAt: new DateTimeOffset(2025, 9, 25, 0, 0, 0, TimeSpan.Zero),
                    ModifiedAt: new DateTimeOffset(2025, 9, 26, 0, 0, 0, TimeSpan.Zero),
                    DeletedAt: new DateTimeOffset(2025, 9, 26, 0, 0, 0, TimeSpan.Zero),
                    Version: 10
                ),
                [new NamespaceWasRestored()]
            ),
            new(
                new RestoreNamespaceCommand(),
                new NamespaceAggregate(
                    Name: "my-namespace",
                    DisplayName: "Old namespace",
                    Description: "Some old description",
                    Documentation: "# Namespace old Documentation",
                    Status: NamespaceStatus.Active,
                    CreatedAt: new DateTimeOffset(2025, 9, 25, 0, 0, 0, TimeSpan.Zero),
                    ModifiedAt: new DateTimeOffset(2025, 9, 26, 0, 0, 0, TimeSpan.Zero),
                    DeletedAt: null,
                    Version: 10
                ),
                []
            ),
        ];

    // GetById

    [Theory]
    [MemberData(nameof(GetByIdTestData))]
    public void GetByIdTest(ByIdTestData test)
    {
        var actual = test.Query.GetDetailsInfo(test.Aggregate);

        actual.ShouldBe(test.Expected);
    }

    public sealed record ByIdTestData(
        GetNamespaceByIdQuery Query,
        NamespaceAggregate Aggregate,
        NamespaceDetailsInfo Expected
    );

    public static TheoryData<ByIdTestData> GetByIdTestData() =>
        [
            new(
                new GetNamespaceByIdQuery(),
                new NamespaceAggregate(
                    Name: "my-namespace",
                    DisplayName: "My namespace",
                    Description: "Some description",
                    Documentation: "# Namespace Documentation",
                    Status: NamespaceStatus.Active,
                    CreatedAt: new DateTimeOffset(2025, 9, 25, 0, 0, 0, TimeSpan.Zero),
                    ModifiedAt: new DateTimeOffset(2025, 9, 26, 0, 0, 0, TimeSpan.Zero),
                    DeletedAt: null,
                    Version: 10
                ),
                new NamespaceDetailsInfo(
                    new NamespaceDetails(
                        Name: "my-namespace",
                        DisplayName: "My namespace",
                        Description: "Some description",
                        Documentation: "# Namespace Documentation",
                        Status: NamespaceStatus.Active,
                        CreatedAt: new DateTimeOffset(2025, 9, 25, 0, 0, 0, TimeSpan.Zero),
                        ModifiedAt: new DateTimeOffset(2025, 9, 26, 0, 0, 0, TimeSpan.Zero),
                        DeletedAt: null,
                        Version: 10
                    ),
                    new NamespaceOperations(
                        CanUpdateDescriptions: true,
                        CanUpdateDocumentation: true,
                        CanDelete: true,
                        CanRestore: false
                    )
                )
            ),
        ];
}
