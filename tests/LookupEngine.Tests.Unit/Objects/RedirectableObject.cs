using JetBrains.Annotations;

namespace LookupEngine.Tests.Unit.Objects;

[PublicAPI]
public sealed class RedirectableObject
{
    public RedirectableObject? Property { get; set; }
}