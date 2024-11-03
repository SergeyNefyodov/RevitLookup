using System.Diagnostics;
using JetBrains.Annotations;
using LookupEngine.Abstractions.Enums;

// ReSharper disable once CheckNamespace
namespace LookupEngine.Abstractions;

[PublicAPI]
[DebuggerDisplay("Name = {Name} Value = {Value}")]
public sealed class DecompositionMemberData
{
    public int Depth { get; set; }
    public required string Name { get; set; }
    public required object? Value { get; set; }
    public required string Type { get; set; }
    public required string? TypeFullName { get; set; }
    public string? Description { get; set; }
    public double ComputationTime { get; set; }
    public long AllocatedBytes { get; set; }
    public MemberAttributes MemberAttributes { get; set; }
}