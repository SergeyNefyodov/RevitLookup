using RevitLookup.Abstractions.ObservableModels.Decomposition;

namespace RevitLookup.Abstractions.Services.Decomposition;

public interface IDecompositionSearchService
{
    (List<ObservableDecomposedObject> FilteredObjects, List<ObservableDecomposedMember> FilteredMembers) Search(
        string query,
        ObservableDecomposedObject? selectedObject,
        List<ObservableDecomposedObject> objects);

    List<ObservableDecomposedMember> SearchMembers(string query, ObservableDecomposedObject value);
}