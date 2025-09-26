using System.Runtime.CompilerServices;
using DiffEngine;

namespace SchemaRegistry.WebApi.UnitTests;

internal class ModuleInitialization
{
    [ModuleInitializer]
    internal static void Init()
    {
        DiffTools.UseOrder(DiffTool.VisualStudioCode);

        VerifierSettings.DontIgnoreEmptyCollections();
    }
}
