using System.Collections;
using RevitLookup.Abstractions.Models.Decomposition;
using RevitLookup.Abstractions.ObservableModels.Decomposition;

namespace RevitLookup.Abstractions.Services.Decomposition;

public interface IVisualDecompositionService
{
    Task VisualizeDecompositionAsync(KnownDecompositionObject decompositionObject);
    Task VisualizeDecompositionAsync(object? obj);
    Task VisualizeDecompositionAsync(IEnumerable objects);
    Task VisualizeDecompositionAsync(ObservableDecomposedObject decomposedObject);
    Task VisualizeDecompositionAsync(List<ObservableDecomposedObject> decomposedObjects);
}