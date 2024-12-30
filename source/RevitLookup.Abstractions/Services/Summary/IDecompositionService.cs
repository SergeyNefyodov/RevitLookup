using System.Collections;
using RevitLookup.Abstractions.ObservableModels.Decomposition;

namespace RevitLookup.Abstractions.Services.Summary;

public interface IDecompositionService
{
    Task<ObservableDecomposedObject> DecomposeAsync(object obj);
    Task<List<ObservableDecomposedObject>> DecomposeAsync(IEnumerable objects);
    Task<List<ObservableDecomposedMember>> DecomposeMembersAsync(ObservableDecomposedObject decomposedObject);
}