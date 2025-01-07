using System.Collections;
using System.Diagnostics.CodeAnalysis;
using LookupEngine;
using LookupEngine.Options;
using RevitLookup.Abstractions.ObservableModels.Decomposition;
using RevitLookup.Abstractions.Services.Decomposition;
using RevitLookup.Abstractions.Services.Settings;
using RevitLookup.Core;
using RevitLookup.Core.Decomposition;
using RevitLookup.Mappers;

namespace RevitLookup.Services.Summary;

[SuppressMessage("ReSharper", "LoopCanBeConvertedToQuery")]
[SuppressMessage("ReSharper", "ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator")]
public sealed class DecompositionService(ISettingsService settingsService) : IDecompositionService
{
    public async Task<ObservableDecomposedObject> DecomposeAsync(object obj)
    {
        var options = CreateDecomposeMembersOptions();
        return await RevitShell.AsyncObjectHandler.RaiseAsync(_ =>
        {
            var result = LookupComposer.Decompose(obj, options);
            return DecompositionResultMapper.Convert(result);
        });
    }

    public async Task<List<ObservableDecomposedObject>> DecomposeAsync(IEnumerable objects)
    {
        var options = CreateDecomposeOptions();
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

    public async Task<List<ObservableDecomposedMember>> DecomposeMembersAsync(ObservableDecomposedObject decomposedObject)
    {
        var options = CreateDecomposeMembersOptions();
        return await RevitShell.AsyncMembersHandler.RaiseAsync(_ =>
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

    private static DecomposeOptions<Document> CreateDecomposeOptions()
    {
        return new DecomposeOptions<Document>
        {
            Context = Context.ActiveDocument!, //TODO: replace
            EnableRedirection = true,
            TypeResolver = DescriptorsMap.FindDescriptor
        };
    }

    private DecomposeOptions<Document> CreateDecomposeMembersOptions()
    {
        return new DecomposeOptions<Document>
        {
            Context = Context.ActiveDocument!, //TODO: replace
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