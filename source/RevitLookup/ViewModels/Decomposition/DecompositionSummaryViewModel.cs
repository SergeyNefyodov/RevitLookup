using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;
using RevitLookup.Abstractions.ObservableModels.Decomposition;
using RevitLookup.Abstractions.Services.Application;
using RevitLookup.Abstractions.Services.Decomposition;
using RevitLookup.Abstractions.Services.Presentation;
using RevitLookup.Abstractions.ViewModels.Decomposition;
using RevitLookup.UI.Framework.Extensions;
using RevitLookup.UI.Framework.Views.Decomposition;

namespace RevitLookup.ViewModels.Decomposition;

[UsedImplicitly]
public sealed partial class DecompositionSummaryViewModel(
    IWindowIntercomService intercomService,
    IDecompositionService decompositionService,
    INotificationService notificationService,
    ILogger<DecompositionSummaryViewModel> logger)
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
        SearchText = string.Empty;
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

        value.Members = await decompositionService.DecomposeMembersAsync(value);
    }

    [Pure]
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