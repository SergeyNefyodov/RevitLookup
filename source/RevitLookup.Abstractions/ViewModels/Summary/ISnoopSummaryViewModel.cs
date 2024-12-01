using RevitLookup.Abstractions.ObservableModels.Decomposition;

namespace RevitLookup.Abstractions.ViewModels.Summary;

public interface ISnoopSummaryViewModel
{
    ObservableDecomposedObject? SelectedDecomposedObject { get; set; }
    List<ObservableDecomposedObject> DecomposedObjects { get; }
    List<ObservableDecomposedObjectsGroup> FilteredDecomposedObjects { get; }
    List<ObservableDecomposedMember> Members { get; set; }

    List<ObservableDecomposedMember> FilteredMembers { get; }

    // IAsyncRelayCommand FetchMembersCommand { get; }
    // IAsyncRelayCommand RefreshMembersCommand { get; }
    string SearchText { get; set; }
    // public IServiceProvider ServiceProvider { get; }
    // void Navigate(SnoopableObject selectedItem);
    // void Navigate(IList<SnoopableObject> selectedItems);
    // void RemoveObject(object obj);
}