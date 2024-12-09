using CommunityToolkit.Mvvm.ComponentModel;
using LookupEngine.Abstractions.ComponentModel;

namespace RevitLookup.Abstractions.ObservableModels.Decomposition;

public sealed partial class ObservableDecomposedObject : ObservableObject
{
    [ObservableProperty] private List<ObservableDecomposedMember> _members = [];
    [ObservableProperty] private List<ObservableDecomposedMember> _filteredMembers = [];

    public required object? RawValue { get; init; }
    public required string Name { get; init; }
    public required string TypeName { get; set; }
    public required string TypeFullName { get; set; }
    public string? Description { get; init; }
    public Descriptor? Descriptor { get; init; }

    partial void OnMembersChanged(List<ObservableDecomposedMember> value)
    {
        FilteredMembers = value;
    }
}