using System.Diagnostics;
using JetBrains.Annotations;
using LookupEngine.Abstractions.Enums;

// ReSharper disable once CheckNamespace
namespace LookupEngine.Abstractions;

[PublicAPI]
[DebuggerDisplay("Name = {Name} Value = {Value.Name}")]
public sealed class DecomposedMember
{
    public required int Depth { get; init; }
    public required string Name { get; init; }
    public required string DeclaringTypeName { get; init; }
    public required string DeclaringTypeFullName { get; init; }
    public double ComputationTime { get; init; }
    public long AllocatedBytes { get; init; }
    public MemberAttributes MemberAttributes { get; init; }
    public required DecomposedValue Value { get; init; }
}