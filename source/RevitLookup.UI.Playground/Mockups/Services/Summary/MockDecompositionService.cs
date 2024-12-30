using System.Collections;
using System.Diagnostics.CodeAnalysis;
using LookupEngine;
using RevitLookup.Abstractions.ObservableModels.Decomposition;
using RevitLookup.Abstractions.Services.Settings;
using RevitLookup.Abstractions.Services.Summary;
using RevitLookup.UI.Playground.Mockups.Core.Summary;
using RevitLookup.UI.Playground.Mockups.Mappers;

namespace RevitLookup.UI.Playground.Mockups.Services.Summary;

[SuppressMessage("ReSharper", "LoopCanBeConvertedToQuery")]
[SuppressMessage("ReSharper", "ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator")]
public sealed class MockDecompositionService(ISettingsService settingsService) : IDecompositionService
{
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