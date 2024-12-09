using CommunityToolkit.Mvvm.Input;
using RevitLookup.Abstractions.ObservableModels.Decomposition;

namespace RevitLookup.Abstractions.ViewModels.Summary;

public interface ISnoopSummaryViewModel
{
    string SearchText { get; set; }

    //Objects
    ObservableDecomposedObject? SelectedDecomposedObject { get; set; }
    List<ObservableDecomposedObject> DecomposedObjects { get; set; }
    List<ObservableDecomposedObjectsGroup> FilteredDecomposedObjects { get; }

    //Commands
    IAsyncRelayCommand RefreshMembersCommand { get; }

    //Navigation
    void Navigate(object? value);
    void Navigate(ObservableDecomposedObject value);
    void Navigate(List<ObservableDecomposedObject> values);

    // void RemoveObject(object obj);
}