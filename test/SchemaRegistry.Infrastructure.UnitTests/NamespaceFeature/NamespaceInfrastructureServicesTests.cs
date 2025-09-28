using Marten;
using Microsoft.Extensions.DependencyInjection;
using Pico.DependencyInjection;
using Pico.DependencyInjection.Testing;
using Pico.Reflection;
using Pico.Testing;
using SchemaRegistry.Infrastructure.NamespaceFeature;

namespace SchemaRegistry.Infrastructure.UnitTests.NamespaceFeature;

[UnitTest]
public class NamespaceInfrastructureServicesTests
{
    [Fact]
    public async Task AddNamespaceOrleansServicesTest()
    {
        var services = new ServiceCollection();

        var capture = services
            .CaptureRegisteredServices(s => s.AddNamespaceOrleansServices())
            .ForTesting(new ToDisplayNameOptions(FullNames: true));

        await Verify(capture).UseSnapshotsDirectory();
    }
}
