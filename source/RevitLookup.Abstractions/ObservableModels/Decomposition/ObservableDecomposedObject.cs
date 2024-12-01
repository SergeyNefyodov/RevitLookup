using CommunityToolkit.Mvvm.ComponentModel;

namespace RevitLookup.Abstractions.ObservableModels.Decomposition;

public sealed class ObservableDecomposedObject : ObservableObject
{
    public required object? Value { get; set; }
    public required string Name { get; set; }
    public required string Type { get; set; }
    public required string TypeFullName { get; set; }
    public required List<ObservableDecomposedMember> Members { get; set; }
}