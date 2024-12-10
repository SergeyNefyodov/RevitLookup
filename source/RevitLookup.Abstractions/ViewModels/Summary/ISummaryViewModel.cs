using CommunityToolkit.Mvvm.Input;
using RevitLookup.Abstractions.ObservableModels.Decomposition;

namespace RevitLookup.Abstractions.ViewModels.Summary;

public interface ISummaryViewModel
{
    string SearchText { get; set; }

    //Objects
    ObservableDecomposedObject? SelectedDecomposedObject { get; set; }
    List<ObservableDecomposedObject> DecomposedObjects { get; set; }

    //Commands
    IAsyncRelayCommand RefreshMembersCommand { get; }

    //Navigation
    void Navigate(object? value);
    void Navigate(ObservableDecomposedObject value);
    void Navigate(List<ObservableDecomposedObject> values);
}