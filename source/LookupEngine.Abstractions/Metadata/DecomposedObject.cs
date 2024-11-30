using System.Diagnostics;
using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace LookupEngine.Abstractions;

[PublicAPI]
[DebuggerDisplay("Name = {Name} Value = {Value}")]
public sealed class DecomposedObject
{
    public required object? Value { get; init; }
    public required string Name { get; init; }
    public required string Type { get; set; }
    public required string TypeFullName { get; set; }
    public required List<DecomposedMember> Members { get; init; }
}