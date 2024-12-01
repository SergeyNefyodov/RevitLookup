using CommunityToolkit.Mvvm.ComponentModel;

namespace RevitLookup.Abstractions.ObservableModels.Decomposition;

public sealed class ObservableDecomposedObjectsGroup : ObservableObject
{
    public required string GroupName { get; set; }
    public required List<ObservableDecomposedObject> GroupItems { get; set; }
}