using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using LookupEngine;
using RevitLookup.Abstractions.Models.Summary;
using RevitLookup.Abstractions.ObservableModels.Decomposition;
using RevitLookup.Abstractions.Services;
using RevitLookup.Abstractions.Services.Presentation;
using RevitLookup.Abstractions.Services.Summary;
using RevitLookup.Abstractions.ViewModels.Summary;
using RevitLookup.UI.Playground.Mappers;

namespace RevitLookup.UI.Playground.Services.Summary;

public sealed class MockVisualDecompositionService(
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
                    await Task.Delay(1000);
                    break;
            }

            summaryViewModel.DecomposedObjects = await DecomposeAsync(new object[] { decompositionObject });
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
        summaryViewModel.DecomposedObjects = await DecomposeAsync(new[] { obj });
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
    private async Task<List<ObservableDecomposedObject>> DecomposeAsync(IEnumerable objects)
    {
        return await Task.Run(() =>
        {
            var count = objects is ICollection collection ? collection.Count : 4;
            var decomposedObjects = new List<ObservableDecomposedObject>(count);
            foreach (var obj in objects)
            {
                var decomposedObject = LookupComposer.DecomposeObject(obj);
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