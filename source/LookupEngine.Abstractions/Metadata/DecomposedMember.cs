using System.Diagnostics;
using JetBrains.Annotations;
using LookupEngine.Abstractions.Enums;

// ReSharper disable once CheckNamespace
namespace LookupEngine.Abstractions;

[PublicAPI]
[DebuggerDisplay("Name = {Name} Value = {Value.Name}")]
public sealed class DecomposedMember
{
    public required DecomposedValue Value { get; init; }
    public required int Depth { get; init; }
    public required string Name { get; init; }
    public required string DeclaringTypeName { get; init; }
    public required string DeclaringTypeFullName { get; init; }
    public string? Description { get; set; }
    public double ComputationTime { get; set; }
    public long AllocatedBytes { get; set; }
    public MemberAttributes MemberAttributes { get; set; }
}