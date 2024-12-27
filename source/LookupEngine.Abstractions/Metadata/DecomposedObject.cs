using System.Diagnostics;
using JetBrains.Annotations;
using LookupEngine.Abstractions.Decomposition;

// ReSharper disable once CheckNamespace
namespace LookupEngine.Abstractions;

[PublicAPI]
[DebuggerDisplay("Name = {Name} Value = {RawValue}")]
public sealed class DecomposedObject
{
    public required object? RawValue { get; init; }
    public required string Name { get; init; }
    public required string TypeName { get; init; }
    public required string TypeFullName { get; init; }
    public string? Description { get; set; }
    public Descriptor? Descriptor { get; init; }
    public List<DecomposedMember> Members { get; } = [];

    public DecomposedObject WithDescription(string? description)
    {
        Description = description;
        return this;
    }
}