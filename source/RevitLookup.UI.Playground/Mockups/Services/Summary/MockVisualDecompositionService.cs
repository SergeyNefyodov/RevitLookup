using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using LookupEngine;
using LookupEngine.Abstractions.Configuration;
using RevitLookup.Abstractions.Models.Summary;
using RevitLookup.Abstractions.ObservableModels.Decomposition;
using RevitLookup.Abstractions.Services.Presentation;
using RevitLookup.Abstractions.Services.Settings;
using RevitLookup.Abstractions.Services.Summary;
using RevitLookup.Abstractions.ViewModels.Summary;
using RevitLookup.UI.Playground.Mockups.Core.Summary;
using RevitLookup.UI.Playground.Mockups.Mappers;

namespace RevitLookup.UI.Playground.Mockups.Services.Summary;

[SuppressMessage("ReSharper", "LoopCanBeConvertedToQuery")]
[SuppressMessage("ReSharper", "ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator")]
public sealed class MockVisualDecompositionService(
    IWindowIntercomService intercomService,
    INotificationService notificationService,
    ISettingsService settingsService,
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

            summaryViewModel.DecomposedObjects = await DecomposeAsync(new object[] {decompositionObject});
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
        var objects = obj switch
        {
            ObservableDecomposedValue {Descriptor: IDescriptorEnumerator} decomposedValue => (IEnumerable) decomposedValue.RawValue!,
            ObservableDecomposedValue decomposedValue => new[] {decomposedValue.RawValue},
            _ => new[] {obj}
        };

        summaryViewModel.DecomposedObjects = await DecomposeAsync(objects);
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

    public async Task<ObservableDecomposedObject> DecomposeAsync(object obj)
    {
        var options = CreateDecomposeMembersOptions();
        return await Task.Run(() =>
        {
            var result = LookupComposer.Decompose(obj, options);
            return DecompositionResultMapper.Convert(result);
        });
    }

    public async Task<List<ObservableDecomposedObject>> DecomposeAsync(IEnumerable objects)
    {
        var options = CreateDecomposeOptions();
        return await Task.Run(() =>
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

    public async Task<List<ObservableDecomposedMember>> DecomposeMembersAsync(ObservableDecomposedObject decomposedObject)
    {
        var options = CreateDecomposeMembersOptions();
        return await Task.Run(() =>
        {
            var decomposedMembers = LookupComposer.DecomposeMembers(decomposedObject.RawValue, options);
            var members = new List<ObservableDecomposedMember>(decomposedMembers.Count);

            foreach (var decomposedMember in decomposedMembers)
            {
                members.Add(DecompositionResultMapper.Convert(decomposedMember));
            }

            return members;
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


    private static DecomposeOptions CreateDecomposeOptions()
    {
        return new DecomposeOptions
        {
            EnableRedirection = true,
            TypeResolver = DescriptorsMap.FindDescriptor
        };
    }

    private DecomposeOptions CreateDecomposeMembersOptions()
    {
        return new DecomposeOptions
        {
            IncludeRoot = settingsService.GeneralSettings.IncludeRootHierarchy,
            IncludeFields = settingsService.GeneralSettings.IncludeFields,
            IncludeEvents = settingsService.GeneralSettings.IncludeEvents,
            IncludeUnsupported = settingsService.GeneralSettings.IncludeUnsupported,
            IncludePrivateMembers = settingsService.GeneralSettings.IncludePrivate,
            IncludeStaticMembers = settingsService.GeneralSettings.IncludeStatic,
            EnableExtensions = settingsService.GeneralSettings.IncludeExtensions,
            EnableRedirection = true,
            TypeResolver = DescriptorsMap.FindDescriptor
        };
    }
}