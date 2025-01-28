using System.Collections.ObjectModel;
using RevitLookup.Abstractions.ObservableModels.Decomposition;

namespace RevitLookup.Abstractions.ViewModels.Decomposition;

public interface IDecompositionSummaryViewModel : ISummaryViewModel
{
    ObservableCollection<ObservableDecomposedObjectsGroup> FilteredDecomposedObjects { get; }
    void RemoveItem(object target);
}