using CommunityToolkit.Mvvm.ComponentModel;
using LookupEngine.Abstractions.Enums;

namespace RevitLookup.Abstractions.ObservableModels.Decomposition;

public sealed class ObservableDecomposedMember : ObservableObject
{
    public required int Depth { get; init; }
    public required object? Value { get; init; }
    public required string Name { get; init; }
    public required string Type { get; init; }
    public required string TypeFullName { get; init; }
    public required string? Description { get; init; }
    public required double ComputationTime { get; init; }
    public required long AllocatedBytes { get; init; }
    public required MemberAttributes MemberAttributes { get; init; }
}