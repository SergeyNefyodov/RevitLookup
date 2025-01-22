using System.Collections;
using RevitLookup.Abstractions.ObservableModels.Decomposition;

namespace RevitLookup.Abstractions.Services.Decomposition;

public interface IDecompositionService
{
    List<ObservableDecomposedObject> DecompositionStackHistory { get; }
    Task<ObservableDecomposedObject> DecomposeAsync(object? obj);
    Task<List<ObservableDecomposedObject>> DecomposeAsync(IEnumerable objects);
    Task<List<ObservableDecomposedMember>> DecomposeMembersAsync(ObservableDecomposedObject decomposedObject);
}