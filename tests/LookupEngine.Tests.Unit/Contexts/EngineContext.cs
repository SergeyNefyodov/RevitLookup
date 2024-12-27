using JetBrains.Annotations;

namespace LookupEngine.Tests.Unit.Contexts;

[PublicAPI]
public sealed class EngineContext
{
    public int Version { get; } = 1;
    public string Metadata { get; } = "Test context";
}