using System.Diagnostics;
using JetBrains.Annotations;
using LookupEngine.Abstractions.ComponentModel;

// ReSharper disable once CheckNamespace
namespace LookupEngine.Abstractions;

[PublicAPI]
[DebuggerDisplay("Name = {Name} Value = {RawValue}")]
public sealed class DecomposedValue
{
    public required object? RawValue { get; set; }
    public required string Name { get; set; }
    public required string TypeName { get; set; }
    public required string TypeFullName { get; set; }
    public Descriptor? Descriptor { get; init; }
}