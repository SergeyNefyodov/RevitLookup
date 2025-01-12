using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Mvvm.ComponentModel;
using JetBrains.Annotations;
using LookupEngine;
using Microsoft.Extensions.Logging;
using RevitLookup.Abstractions.ObservableModels.Decomposition;
using RevitLookup.Abstractions.Services.Application;
using RevitLookup.Abstractions.Services.Presentation;
using RevitLookup.Abstractions.Services.Settings;
using RevitLookup.Abstractions.ViewModels.Decomposition;
using RevitLookup.UI.Framework.Extensions;
using RevitLookup.UI.Framework.Views.Decomposition;
using RevitLookup.UI.Playground.Mockups.Core.Decomposition;
using RevitLookup.UI.Playground.Mockups.Mappers;
#if NETFRAMEWORK
using RevitLookup.UI.Framework.Extensions;
#endif

namespace RevitLookup.UI.Playground.Mockups.ViewModels.Decomposition;

[UsedImplicitly]
public sealed partial class MockDecompositionSummaryViewModel(
    ISettingsService settingsService,
    IWindowIntercomService intercomService,
    INotificationService notificationService,
    ILogger<MockDecompositionSummaryViewModel> logger)
    : ObservableObject, IDecompositionSummaryViewModel
{
    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private ObservableDecomposedObject? _selectedDecomposedObject;
    [ObservableProperty] private List<ObservableDecomposedObject> _decomposedObjects = [];
    [ObservableProperty] private ObservableCollection<ObservableDecomposedObjectsGroup> _filteredDecomposedObjects = [];

    public void Navigate(object? value)
    {
        Host.GetService<IRevitLookupUiService>()
            .Decompose(value)
            .DependsOn(intercomService.GetHost())
            .Show<DecompositionSummaryPage>();
    }

    public void Navigate(ObservableDecomposedObject value)
    {
        Host.GetService<IRevitLookupUiService>()
            .Decompose(value)
            .DependsOn(intercomService.GetHost())
            .Show<DecompositionSummaryPage>();
    }

    public void Navigate(List<ObservableDecomposedObject> values)
    {
        Host.GetService<IRevitLookupUiService>()
            .Decompose(values)
            .DependsOn(intercomService.GetHost())
            .Show<DecompositionSummaryPage>();
    }

    public async Task RefreshMembersAsync()
    {
        foreach (var decomposedObject in DecomposedObjects)
        {
            decomposedObject.Members.Clear();
        }

        try
        {
            await FetchMembersAsync(SelectedDecomposedObject);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Members decomposing failed");
            notificationService.ShowError("Lookup engine error", exception);
        }
    }

    public void RemoveItem(object target)
    {
        switch (target)
        {
            case ObservableDecomposedObject decomposedObject:
                for (var i = FilteredDecomposedObjects.Count - 1; i >= 0; i--)
                {
                    var groupToRemove = FilteredDecomposedObjects[i];
                    if (!groupToRemove.GroupItems.Remove(decomposedObject)) continue;

                    if (groupToRemove.GroupItems.Count == 0)
                    {
                        //Remove the empty group
                        FilteredDecomposedObjects.Remove(groupToRemove);
                    }
                }

                if (DecomposedObjects.Remove(decomposedObject))
                {
                    if (DecomposedObjects.Count == 0)
                    {
                        //Notify UI to update placeholders
                        OnPropertyChanged(nameof(DecomposedObjects));
                    }
                }

                break;
            case ObservableDecomposedMember:
                //Do nothing ??
                break;
        }
    }

    partial void OnDecomposedObjectsChanged(List<ObservableDecomposedObject> value)
    {
        OnSearchTextChanged(SearchText);
    }

    async partial void OnSearchTextChanged(string value)
    {
        try
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                FilteredDecomposedObjects = ApplyGrouping(DecomposedObjects);
                return;
            }

            FilteredDecomposedObjects = await Task.Run(() =>
            {
                var formattedText = value.Trim();
                var searchResults = new List<ObservableDecomposedObject>();
                // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
                foreach (var item in DecomposedObjects)
                {
                    if (item.Name.Contains(formattedText, StringComparison.OrdinalIgnoreCase) || item.Name.Contains(formattedText, StringComparison.OrdinalIgnoreCase))
                    {
                        searchResults.Add(item);
                    }
                }

                return ApplyGrouping(searchResults);
            });

            if (FilteredDecomposedObjects.Count == 0)
            {
                SelectedDecomposedObject = null;
            }
        }
        catch
        {
            // ignored
        }
    }

    async partial void OnSelectedDecomposedObjectChanged(ObservableDecomposedObject? value)
    {
        try
        {
            await FetchMembersAsync(value);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Members decomposing failed");
            notificationService.ShowError("Lookup engine error", exception);
        }
    }

    private async Task FetchMembersAsync(ObservableDecomposedObject? value)
    {
        if (value is null) return;
        if (value.Members.Count > 0) return;

        value.Members = await DecomposeMembersAsync(value);
    }

    [SuppressMessage("ReSharper", "ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator")]
    private async Task<List<ObservableDecomposedMember>> DecomposeMembersAsync(ObservableDecomposedObject decomposedObject)
    {
        var options = new DecomposeOptions
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

    private ObservableCollection<ObservableDecomposedObjectsGroup> ApplyGrouping(List<ObservableDecomposedObject> objects)
    {
        return objects
            .OrderBy(data => data.TypeName)
            .ThenBy(data => data.Name)
            .GroupBy(data => data.TypeName)
            .Select(group => new ObservableDecomposedObjectsGroup
            {
                GroupName = group.Key,
                GroupItems = group.ToObservableCollection()
            })
            .ToObservableCollection();
    }
}