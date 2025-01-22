using System.Collections;
using System.Windows.Controls;
using RevitLookup.Abstractions.Models.Decomposition;
using RevitLookup.Abstractions.ObservableModels.Decomposition;

namespace RevitLookup.Abstractions.Services.Application;

public interface IRevitLookupUiService : ILookupServiceHistoryStage, ILookupServiceDecomposeStage, ILookupServiceShowStage;

public interface ILookupServiceHistoryStage
{
    ILookupServiceParentStage AddParent(IServiceProvider serviceProvider);
}

public interface ILookupServiceParentStage : ILookupServiceDecomposeStage
{
    ILookupServiceDecomposeStage AddStackHistory(ObservableDecomposedObject item);
}

public interface ILookupServiceDecomposeStage
{
    ILookupServiceShowStage Decompose(KnownDecompositionObject knownObject);
    ILookupServiceShowStage Decompose(object? input);
    ILookupServiceShowStage Decompose(IEnumerable input);
    ILookupServiceShowStage Decompose(ObservableDecomposedObject decomposedObject);
    ILookupServiceShowStage Decompose(List<ObservableDecomposedObject> decomposedObjects);
}

public interface ILookupServiceShowStage
{
    ILookupServiceRunStage Show<T>() where T : Page;
    // ILookupServiceRunStage ShowDialog<T>() where T : Page;
}

public interface ILookupServiceRunStage
{
    void RunService<T>(Action<T> handler) where T : class;
}