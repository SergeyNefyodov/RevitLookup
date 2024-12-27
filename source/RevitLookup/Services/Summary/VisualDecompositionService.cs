using System.Collections;
using System.Diagnostics.CodeAnalysis;
using LookupEngine;
using LookupEngine.Abstractions.Configuration;
using RevitLookup.Abstractions.Models.Summary;
using RevitLookup.Abstractions.ObservableModels.Decomposition;
using RevitLookup.Abstractions.Services.Presentation;
using RevitLookup.Abstractions.Services.Summary;
using RevitLookup.Abstractions.ViewModels.Summary;
using RevitLookup.Core;
using RevitLookup.Core.Summary;
using RevitLookup.Mappers;
using Visibility = System.Windows.Visibility;

namespace RevitLookup.Services.Summary;

public sealed class VisualDecompositionService(
    IWindowIntercomService intercomService,
    INotificationService notificationService,
    IDecompositionSummaryViewModel summaryViewModel)
    : IVisualDecompositionService
{
    public async Task VisualizeDecompositionAsync(KnownDecompositionObject decompositionObject)
    {
        try
        {
            switch (decompositionObject)
            {
                case KnownDecompositionObject.Face:
                case KnownDecompositionObject.Edge:
                case KnownDecompositionObject.LinkedElement:
                case KnownDecompositionObject.Point:
                case KnownDecompositionObject.SubElement:
                    HideHost();
                    break;
            }

            var objects = await RevitShell.AsyncCollectionHandler.RaiseAsync(_ => RevitObjectsCollector.GetObjects(decompositionObject));
            summaryViewModel.SelectedDecomposedObject = null;
            summaryViewModel.DecomposedObjects = await DecomposeAsync(objects);
        }
        catch (OperationCanceledException)
        {
            notificationService.ShowWarning("Operation cancelled", "Operation cancelled by user");
        }
        catch (Exception exception)
        {
            notificationService.ShowError("Operation cancelled", exception);
        }
        finally
        {
            ShowHost();
        }
    }

    public async Task VisualizeDecompositionAsync(object? obj)
    {
        var values = obj switch
        {
            ObservableDecomposedValue {Descriptor: IDescriptorEnumerator} decomposedValue => (IEnumerable) decomposedValue.RawValue!,
            ObservableDecomposedValue decomposedValue => new[] {decomposedValue.RawValue},
            _ => new[] {obj}
        };

        summaryViewModel.DecomposedObjects = await DecomposeAsync(values);
    }

    public async Task VisualizeDecompositionAsync(IEnumerable objects)
    {
        summaryViewModel.DecomposedObjects = await DecomposeAsync(objects);
    }

    public async Task VisualizeDecompositionAsync(ObservableDecomposedObject decomposedObject)
    {
        summaryViewModel.DecomposedObjects = [decomposedObject];
        await Task.CompletedTask;
    }

    public async Task VisualizeDecompositionAsync(List<ObservableDecomposedObject> decomposedObjects)
    {
        summaryViewModel.DecomposedObjects = decomposedObjects;
        await Task.CompletedTask;
    }

    [SuppressMessage("ReSharper", "LoopCanBeConvertedToQuery")]
    private static async Task<List<ObservableDecomposedObject>> DecomposeAsync(IEnumerable objects)
    {
        var options = new DecomposeOptions
        {
            TypeResolver = DescriptorsMap.FindDescriptor
        };

        return await RevitShell.AsyncObjectsHandler.RaiseAsync(_ =>
        {
            var capacity = objects is ICollection collection ? collection.Count : 4;
            var decomposedObjects = new List<ObservableDecomposedObject>(capacity);
            foreach (var obj in objects)
            {
                var decomposedObject = LookupComposer.DecomposeObject(obj, options);
                decomposedObjects.Add(DecompositionResultMapper.Convert(decomposedObject));
            }

            return decomposedObjects;
        });
    }

    private void ShowHost()
    {
        var host = intercomService.GetHost();
        if (!host.IsLoaded) return;

        host.Visibility = Visibility.Visible;
    }

    private void HideHost()
    {
        var host = intercomService.GetHost();
        if (!host.IsLoaded) return;

        host.Visibility = Visibility.Hidden;
    }
}