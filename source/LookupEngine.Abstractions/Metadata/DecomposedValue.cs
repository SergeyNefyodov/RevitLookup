using System.Diagnostics;
using JetBrains.Annotations;
using LookupEngine.Abstractions.Descriptors;

// ReSharper disable once CheckNamespace
namespace LookupEngine.Abstractions;

[PublicAPI]
[DebuggerDisplay("Name = {Name} Value = {RawValue}")]
public sealed class DecomposedValue
{
    public required object? RawValue { get; init; }
    public required string Name { get; init; }
    public required string TypeName { get; init; }
    public required string TypeFullName { get; init; }
    public string? Description { get; init; }
    public Descriptor? Descriptor { get; init; }
}