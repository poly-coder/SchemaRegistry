using Pico.DependencyInjection.Testing;
using Pico.Testing;
using SchemaRegistry.Infrastructure.NamespaceFeature;

namespace SchemaRegistry.Infrastructure.UnitTests.NamespaceFeature;

[UnitTest]
public class NamespaceInfrastructureServicesTests
{
    [Fact]
    public async Task AddNamespaceOrleansServicesTest()
    {
        await PicoDependencyInjectionTestingExtensions.VerifyRegisteredServices(services =>
        {
            services.AddNamespaceOrleansServices();
        });
    }
}
