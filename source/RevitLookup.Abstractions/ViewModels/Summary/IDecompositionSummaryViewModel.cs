using RevitLookup.Abstractions.ObservableModels.Decomposition;

namespace RevitLookup.Abstractions.ViewModels.Summary;

public interface IDecompositionSummaryViewModel : ISummaryViewModel
{
    List<ObservableDecomposedObjectsGroup> FilteredDecomposedObjects { get; }
}