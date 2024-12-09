using System.Collections;
using RevitLookup.Abstractions.Models.Summary;
using RevitLookup.Abstractions.ObservableModels.Decomposition;

namespace RevitLookup.Abstractions.Services;

public interface IVisualDecompositionService
{
    Task VisualizeDecompositionAsync(KnownDecompositionObject decompositionObject);
    Task VisualizeDecompositionAsync(object? obj);
    Task VisualizeDecompositionAsync(IEnumerable objects);
    Task VisualizeDecompositionAsync(ObservableDecomposedObject decomposedObject);
    Task VisualizeDecompositionAsync(List<ObservableDecomposedObject> decomposedObjects);
}