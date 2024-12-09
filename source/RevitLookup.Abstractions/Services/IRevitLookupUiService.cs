using System.Collections;
using System.Windows;
using System.Windows.Controls;
using RevitLookup.Abstractions.Models.Summary;
using RevitLookup.Abstractions.ObservableModels.Decomposition;

namespace RevitLookup.Abstractions.Services;

public interface IRevitLookupUiService : ILookupServiceDependsStage, ILookupServiceRunStage
{
    ILookupServiceDependsStage Decompose(KnownDecompositionObject decompositionObject);
    ILookupServiceDependsStage Decompose(object? obj);
    ILookupServiceDependsStage Decompose(IEnumerable objects);
    ILookupServiceDependsStage Decompose(ObservableDecomposedObject decomposedObject);
    ILookupServiceDependsStage Decompose(List<ObservableDecomposedObject> decomposedObjects);
}

public interface ILookupServiceDependsStage : ILookupServiceShowStage
{
    ILookupServiceShowStage DependsOn(Window parent);
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