using Xunit.Sdk;

namespace Pico.Testing;

[TraitDiscoverer("Xunit.Sdk.TraitDiscoverer", "xunit.core")]
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public sealed class UnitTestAttribute : Attribute, ITraitAttribute
{
    public UnitTestAttribute(string name = "Category", string value = "UnitTest") { }
}

[TraitDiscoverer("Xunit.Sdk.TraitDiscoverer", "xunit.core")]
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public sealed class IntegrationTestAttribute : Attribute, ITraitAttribute
{
    public IntegrationTestAttribute(string name = "Category", string value = "IntegrationTest") { }
}

[TraitDiscoverer("Xunit.Sdk.TraitDiscoverer", "xunit.core")]
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public sealed class PerformanceTestAttribute : Attribute, ITraitAttribute
{
    public PerformanceTestAttribute(string name = "Category", string value = "UnitTest") { }
}
