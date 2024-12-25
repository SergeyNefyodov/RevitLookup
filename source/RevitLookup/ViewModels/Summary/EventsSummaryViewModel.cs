using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using LookupEngine;
using Microsoft.Extensions.Logging;
using RevitLookup.Abstractions.ObservableModels.Decomposition;
using RevitLookup.Abstractions.Services.Application;
using RevitLookup.Abstractions.Services.Presentation;
using RevitLookup.Abstractions.Services.Settings;
using RevitLookup.Abstractions.ViewModels.Summary;
using RevitLookup.Core;
using RevitLookup.Mappers;
using RevitLookup.Services.Summary;
using RevitLookup.UI.Framework.Extensions;
using RevitLookup.UI.Framework.Views.Summary;

namespace RevitLookup.ViewModels.Summary;

[UsedImplicitly]
public sealed partial class EventsSummaryViewModel(
    ISettingsService settingsService,
    IWindowIntercomService intercomService,
    INotificationService notificationService,
    EventsMonitoringService monitoringService,
    ILogger<DecompositionSummaryViewModel> logger)
    : ObservableObject, IEventsSummaryViewModel
{
    private readonly SynchronizationContext _synchronizationContext = SynchronizationContext.Current!;

    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private ObservableDecomposedObject? _selectedDecomposedObject;
    [ObservableProperty] private List<ObservableDecomposedObject> _decomposedObjects = [];
    [ObservableProperty] private ObservableCollection<ObservableDecomposedObject> _filteredDecomposedObjects = [];

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

    public Task OnNavigatedToAsync()
    {
        monitoringService.RegisterEventInvocationCallback(OnEventInvoked);
        return Task.CompletedTask;
    }

    public Task OnNavigatedFromAsync()
    {
        monitoringService.Unregister();
        return Task.CompletedTask;
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
                FilteredDecomposedObjects = DecomposedObjects.ToObservableCollection();
                return;
            }

            FilteredDecomposedObjects = await Task.Run(() =>
            {
                var formattedText = value.Trim();
                var searchResults = new List<ObservableDecomposedObject>();
                // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
                foreach (var item in DecomposedObjects)
                {
                    if (IsSearchValueMatching(item, formattedText))
                    {
                        searchResults.Add(item);
                    }
                }

                return searchResults.ToObservableCollection();
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

    private bool IsSearchValueMatching(ObservableDecomposedObject item, string formattedText)
    {
        return item.Name.Contains(formattedText, StringComparison.OrdinalIgnoreCase) ||
               item.Name.Contains(formattedText, StringComparison.OrdinalIgnoreCase);
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

    private async void OnEventInvoked(object value, string eventName)
    {
        try
        {
            var options = CreateDecomposeOptions();
            var decomposedObject = await RevitShell.AsyncObjectHandler.RaiseAsync(_ =>
            {
                var result = LookupComposer.Decompose(value, options);
                var convertedResult = DecompositionResultMapper.Convert(result);
                convertedResult.Name = $"{eventName} {DateTime.Now:HH:mm:ss}";
                return convertedResult;
            });

            _synchronizationContext.Post(state =>
            {
                var viewModel = (EventsSummaryViewModel) state!;
                viewModel.DecomposedObjects.Insert(0, decomposedObject);

                if (viewModel.IsSearchValueMatching(decomposedObject, viewModel.SearchText))
                {
                    viewModel.FilteredDecomposedObjects.Insert(0, decomposedObject);
                }
                else
                {
                    viewModel.OnSearchTextChanged(viewModel.SearchText);
                }
            }, this);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Events data parsing error");
            notificationService.ShowError("Events data parsing error", exception);
        }
    }

    private async Task FetchMembersAsync(ObservableDecomposedObject? decomposedObject)
    {
        if (decomposedObject is null) return;
        if (decomposedObject.Members.Count > 0) return;

        decomposedObject.Members = await DecomposeMembersAsync(decomposedObject);
    }

    [SuppressMessage("ReSharper", "ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator")]
    private async Task<List<ObservableDecomposedMember>> DecomposeMembersAsync(ObservableDecomposedObject decomposedObject)
    {
        var options = CreateDecomposeOptions();
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

    private DecomposeOptions CreateDecomposeOptions()
    {
        return new DecomposeOptions
        {
            IncludeRoot = settingsService.GeneralSettings.IncludeRootHierarchy,
            IncludeFields = settingsService.GeneralSettings.IncludeFields,
            IncludeEvents = settingsService.GeneralSettings.IncludeEvents,
            IncludeUnsupported = settingsService.GeneralSettings.IncludeUnsupported,
            IncludePrivateMembers = settingsService.GeneralSettings.IncludePrivate,
            IncludeStaticMembers = settingsService.GeneralSettings.IncludeStatic,
            EnableExtensions = settingsService.GeneralSettings.IncludeExtensions
        };
    }
}