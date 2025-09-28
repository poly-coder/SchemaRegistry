using Pico.Testing;
using SchemaRegistry.Infrastructure.NamespaceFeature;

namespace SchemaRegistry.Infrastructure.UnitTests.NamespaceFeature;

[UnitTest]
public class NamespaceGrainUseCasesTests
{
    // Create

    [Theory]
    [MemberData(nameof(GetCreateTestData))]
    public async Task CreateTest(CreateTestData test)
    {
        var grain = new Mock<INamespaceGrain>(MockBehavior.Strict);

        grain
            .Setup(e =>
                e.CreateNamespace(It.IsAny<CreateNamespaceCommand>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(test.Output);

        var factory = new Mock<IGrainFactory>(MockBehavior.Strict);

        factory
            .Setup(e => e.GetGrain<INamespaceGrain>(test.Command.Name, null))
            .Returns(grain.Object);

        using var cts = new CancellationTokenSource();
        var cancel = cts.Token;

        var service = new NamespaceGrainUseCases(factory.Object);

        var result = await service.CreateNamespaceAsync(test.Command, cancel);

        result.ShouldBe(test.ExpectedResult);

        factory.Verify(e => e.GetGrain<INamespaceGrain>(test.Command.Name, null), Times.Once);
        grain.Verify(e => e.CreateNamespace(test.ExpectedInput, cancel), Times.Once);

        factory.VerifyNoOtherCalls();
        grain.VerifyNoOtherCalls();
    }

    public sealed record CreateTestData(
        Domain.NamespaceFeature.CreateNamespaceCommand Command,
        NamespaceCommandResult Output,
        CreateNamespaceCommand ExpectedInput,
        Domain.NamespaceFeature.NamespaceCommandResult ExpectedResult
    );

    public static TheoryData<CreateTestData> GetCreateTestData()
    {
        return
        [
            new(
                Command: new(
                    Name: "my-namespace",
                    DisplayName: "Display name",
                    Description: "Description",
                    Documentation: "# Documentation"
                ),
                Output: new(Updated: true),
                ExpectedInput: new(
                    DisplayName: "Display name",
                    Description: "Description",
                    Documentation: "# Documentation"
                ),
                ExpectedResult: new(Updated: true)
            ),
        ];
    }

    // Update Descriptions

    [Theory]
    [MemberData(nameof(GetUpdateDescriptionsTestData))]
    public async Task UpdateDescriptionsTest(UpdateDescriptionsTestData test)
    {
        var grain = new Mock<INamespaceGrain>(MockBehavior.Strict);

        grain
            .Setup(e =>
                e.UpdateNamespaceDescriptions(
                    It.IsAny<UpdateNamespaceDescriptionsCommand>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(test.Output);

        var factory = new Mock<IGrainFactory>(MockBehavior.Strict);

        factory
            .Setup(e => e.GetGrain<INamespaceGrain>(test.Command.Name, null))
            .Returns(grain.Object);

        using var cts = new CancellationTokenSource();
        var cancel = cts.Token;

        var service = new NamespaceGrainUseCases(factory.Object);

        var result = await service.UpdateNamespaceDescriptionsAsync(test.Command, cancel);

        result.ShouldBe(test.ExpectedResult);

        factory.Verify(e => e.GetGrain<INamespaceGrain>(test.Command.Name, null), Times.Once);
        grain.Verify(e => e.UpdateNamespaceDescriptions(test.ExpectedInput, cancel), Times.Once);

        factory.VerifyNoOtherCalls();
        grain.VerifyNoOtherCalls();
    }

    public sealed record UpdateDescriptionsTestData(
        Domain.NamespaceFeature.UpdateNamespaceDescriptionsCommand Command,
        NamespaceCommandResult Output,
        UpdateNamespaceDescriptionsCommand ExpectedInput,
        Domain.NamespaceFeature.NamespaceCommandResult ExpectedResult
    );

    public static TheoryData<UpdateDescriptionsTestData> GetUpdateDescriptionsTestData()
    {
        return
        [
            new(
                Command: new(
                    Name: "my-namespace",
                    DisplayName: "Display name",
                    Description: "Description"
                ),
                Output: new(Updated: true),
                ExpectedInput: new(DisplayName: "Display name", Description: "Description"),
                ExpectedResult: new(Updated: true)
            ),
        ];
    }

    // Update Documentation

    [Theory]
    [MemberData(nameof(GetUpdateDocumentationTestData))]
    public async Task UpdateDocumentationTest(UpdateDocumentationTestData test)
    {
        var grain = new Mock<INamespaceGrain>(MockBehavior.Strict);

        grain
            .Setup(e =>
                e.UpdateNamespaceDocumentation(
                    It.IsAny<UpdateNamespaceDocumentationCommand>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(test.Output);

        var factory = new Mock<IGrainFactory>(MockBehavior.Strict);

        factory
            .Setup(e => e.GetGrain<INamespaceGrain>(test.Command.Name, null))
            .Returns(grain.Object);

        using var cts = new CancellationTokenSource();
        var cancel = cts.Token;

        var service = new NamespaceGrainUseCases(factory.Object);

        var result = await service.UpdateNamespaceDocumentationAsync(test.Command, cancel);

        result.ShouldBe(test.ExpectedResult);

        factory.Verify(e => e.GetGrain<INamespaceGrain>(test.Command.Name, null), Times.Once);
        grain.Verify(e => e.UpdateNamespaceDocumentation(test.ExpectedInput, cancel), Times.Once);

        factory.VerifyNoOtherCalls();
        grain.VerifyNoOtherCalls();
    }

    public sealed record UpdateDocumentationTestData(
        Domain.NamespaceFeature.UpdateNamespaceDocumentationCommand Command,
        NamespaceCommandResult Output,
        UpdateNamespaceDocumentationCommand ExpectedInput,
        Domain.NamespaceFeature.NamespaceCommandResult ExpectedResult
    );

    public static TheoryData<UpdateDocumentationTestData> GetUpdateDocumentationTestData()
    {
        return
        [
            new(
                Command: new(Name: "my-namespace", Documentation: "# Documentation"),
                Output: new(Updated: true),
                ExpectedInput: new(Documentation: "# Documentation"),
                ExpectedResult: new(Updated: true)
            ),
        ];
    }

    // Delete

    [Theory]
    [MemberData(nameof(GetDeleteTestData))]
    public async Task DeleteTest(DeleteTestData test)
    {
        var grain = new Mock<INamespaceGrain>(MockBehavior.Strict);

        grain
            .Setup(e =>
                e.DeleteNamespace(It.IsAny<DeleteNamespaceCommand>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(test.Output);

        var factory = new Mock<IGrainFactory>(MockBehavior.Strict);

        factory
            .Setup(e => e.GetGrain<INamespaceGrain>(test.Command.Name, null))
            .Returns(grain.Object);

        using var cts = new CancellationTokenSource();
        var cancel = cts.Token;

        var service = new NamespaceGrainUseCases(factory.Object);

        var result = await service.DeleteNamespaceAsync(test.Command, cancel);

        result.ShouldBe(test.ExpectedResult);

        factory.Verify(e => e.GetGrain<INamespaceGrain>(test.Command.Name, null), Times.Once);
        grain.Verify(e => e.DeleteNamespace(test.ExpectedInput, cancel), Times.Once);

        factory.VerifyNoOtherCalls();
        grain.VerifyNoOtherCalls();
    }

    public sealed record DeleteTestData(
        Domain.NamespaceFeature.DeleteNamespaceCommand Command,
        NamespaceCommandResult Output,
        DeleteNamespaceCommand ExpectedInput,
        Domain.NamespaceFeature.NamespaceCommandResult ExpectedResult
    );

    public static TheoryData<DeleteTestData> GetDeleteTestData()
    {
        return
        [
            new(
                Command: new(Name: "my-namespace"),
                Output: new(Updated: true),
                ExpectedInput: new(),
                ExpectedResult: new(Updated: true)
            ),
        ];
    }

    // Restore

    [Theory]
    [MemberData(nameof(GetRestoreTestData))]
    public async Task RestoreTest(RestoreTestData test)
    {
        var grain = new Mock<INamespaceGrain>(MockBehavior.Strict);

        grain
            .Setup(e =>
                e.RestoreNamespace(
                    It.IsAny<RestoreNamespaceCommand>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(test.Output);

        var factory = new Mock<IGrainFactory>(MockBehavior.Strict);

        factory
            .Setup(e => e.GetGrain<INamespaceGrain>(test.Command.Name, null))
            .Returns(grain.Object);

        using var cts = new CancellationTokenSource();
        var cancel = cts.Token;

        var service = new NamespaceGrainUseCases(factory.Object);

        var result = await service.RestoreNamespaceAsync(test.Command, cancel);

        result.ShouldBe(test.ExpectedResult);

        factory.Verify(e => e.GetGrain<INamespaceGrain>(test.Command.Name, null), Times.Once);
        grain.Verify(e => e.RestoreNamespace(test.ExpectedInput, cancel), Times.Once);

        factory.VerifyNoOtherCalls();
        grain.VerifyNoOtherCalls();
    }

    public sealed record RestoreTestData(
        Domain.NamespaceFeature.RestoreNamespaceCommand Command,
        NamespaceCommandResult Output,
        RestoreNamespaceCommand ExpectedInput,
        Domain.NamespaceFeature.NamespaceCommandResult ExpectedResult
    );

    public static TheoryData<RestoreTestData> GetRestoreTestData()
    {
        return
        [
            new(
                Command: new(Name: "my-namespace"),
                Output: new(Updated: true),
                ExpectedInput: new(),
                ExpectedResult: new(Updated: true)
            ),
        ];
    }

    // GetById

    [Theory]
    [MemberData(nameof(GetGetByIdTestData))]
    public async Task GetByIdTest(GetByIdTestData test)
    {
        var grain = new Mock<INamespaceGrain>(MockBehavior.Strict);

        grain
            .Setup(e =>
                e.GetNamespaceById(It.IsAny<GetNamespaceByIdQuery>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(test.Output);

        var factory = new Mock<IGrainFactory>(MockBehavior.Strict);

        factory
            .Setup(e => e.GetGrain<INamespaceGrain>(test.Query.Name, null))
            .Returns(grain.Object);

        using var cts = new CancellationTokenSource();
        var cancel = cts.Token;

        var service = new NamespaceGrainUseCases(factory.Object);

        var result = await service.GetNamespaceByIdAsync(test.Query, cancel);

        result.ShouldBe(test.ExpectedResult);

        factory.Verify(e => e.GetGrain<INamespaceGrain>(test.Query.Name, null), Times.Once);
        grain.Verify(e => e.GetNamespaceById(test.ExpectedInput, cancel), Times.Once);

        factory.VerifyNoOtherCalls();
        grain.VerifyNoOtherCalls();
    }

    public sealed record GetByIdTestData(
        Domain.NamespaceFeature.GetNamespaceByIdQuery Query,
        GetNamespaceByIdQueryResult Output,
        GetNamespaceByIdQuery ExpectedInput,
        Domain.NamespaceFeature.GetNamespaceByIdQueryResult ExpectedResult
    );

    public static TheoryData<GetByIdTestData> GetGetByIdTestData()
    {
        var createdAt = new DateTimeOffset(2025, 9, 28, 12, 0, 0, TimeSpan.Zero);
        var modifiedAt = createdAt.AddDays(3);

        return
        [
            new(
                Query: new(Name: "my-namespace", Deleted: true),
                Output: new(
                    Namespace: new(
                        Details: new(
                            Name: "my-namespace",
                            DisplayName: "Display name",
                            Description: "Description",
                            Documentation: "# Documentation",
                            Status: NamespaceStatus.Active,
                            CreatedAt: createdAt,
                            ModifiedAt: modifiedAt,
                            DeletedAt: null,
                            Version: 42
                        ),
                        Operations: new(
                            CanUpdateDescriptions: true,
                            CanUpdateDocumentation: false,
                            CanDelete: true,
                            CanRestore: false
                        )
                    )
                ),
                ExpectedInput: new(Deleted: true),
                ExpectedResult: new(
                    Namespace: new(
                        Details: new(
                            Name: "my-namespace",
                            DisplayName: "Display name",
                            Description: "Description",
                            Documentation: "# Documentation",
                            Status: Domain.NamespaceFeature.NamespaceStatus.Active,
                            CreatedAt: createdAt,
                            ModifiedAt: modifiedAt,
                            DeletedAt: null,
                            Version: 42
                        ),
                        Operations: new(
                            CanUpdateDescriptions: true,
                            CanUpdateDocumentation: false,
                            CanDelete: true,
                            CanRestore: false
                        )
                    )
                )
            ),
        ];
    }
}
