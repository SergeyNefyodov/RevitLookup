using System.Diagnostics;
using JetBrains.Annotations;
using LookupEngine.Abstractions.Enums;

// ReSharper disable once CheckNamespace
namespace LookupEngine.Abstractions;

[PublicAPI]
[DebuggerDisplay("Name = {Name} Value = {Value.Name}")]
public sealed class DecomposedMember
{
    public required DecomposedValue Value { get; set; }
    public required int Depth { get; set; }
    public required string Name { get; set; }
    public required string DeclaringTypeName { get; set; }
    public required string DeclaringTypeFullName { get; set; }
    public string? Description { get; set; }
    public double ComputationTime { get; set; }
    public long AllocatedBytes { get; set; }
    public MemberAttributes MemberAttributes { get; set; }
}