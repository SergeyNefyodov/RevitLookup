using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Media;
using Bogus;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JetBrains.Annotations;
using LookupEngine;
using Microsoft.Extensions.Logging;
using RevitLookup.Abstractions.ObservableModels.Decomposition;
using RevitLookup.Abstractions.Services;
using RevitLookup.Abstractions.ViewModels.Summary;
using RevitLookup.UI.Framework.Extensions;
using RevitLookup.UI.Framework.Views.Summary;
using RevitLookup.UI.Playground.Mappers;
#if NETFRAMEWORK
using RevitLookup.UI.Framework.Extensions;
#endif

namespace RevitLookup.UI.Playground.ViewModels.Summary;

[UsedImplicitly]
public sealed partial class MockEventsSummaryViewModel(
    ISettingsService settingsService,
    IWindowIntercomService intercomService,
    INotificationService notificationService,
    ILogger<MockDecompositionSummaryViewModel> logger)
    : ObservableObject, IEventsSummaryViewModel
{
    private CancellationTokenSource? _cancellationTokenSource;

    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private ObservableDecomposedObject? _selectedDecomposedObject;
    [ObservableProperty] private List<ObservableDecomposedObject> _decomposedObjects = [];
    [ObservableProperty] private ObservableCollection<ObservableDecomposedObject> _filteredDecomposedObjects = [];

    [RelayCommand]
    private async Task RefreshMembersAsync()
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

    public async Task OnNavigatedToAsync()
    {
        _cancellationTokenSource = new CancellationTokenSource();

        var faker = new Faker();
        var cancellationToken = _cancellationTokenSource.Token;
        while (!cancellationToken.IsCancellationRequested)
        {
            var decomposedObject = GenerateRandomObject(faker);
            DecomposedObjects.Insert(0, decomposedObject);

            if (SearchText == string.Empty) FilteredDecomposedObjects.Insert(0, decomposedObject);
            else OnSearchTextChanged(SearchText);

            await Task.Delay(1000, cancellationToken);
        }
    }

    public Task OnNavigatedFromAsync()
    {
        if (_cancellationTokenSource is null) return Task.CompletedTask;

        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
        _cancellationTokenSource = null;

        return Task.CompletedTask;
    }

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
                    if (item.Name.Contains(formattedText, StringComparison.OrdinalIgnoreCase) || item.Name.Contains(formattedText, StringComparison.OrdinalIgnoreCase))
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

    private async Task FetchMembersAsync(ObservableDecomposedObject? decomposedObject)
    {
        if (decomposedObject is null) return;
        if (decomposedObject.Members.Count > 0) return;

        decomposedObject.Members = await DecomposeMembersAsync(decomposedObject);
    }

    [SuppressMessage("ReSharper", "ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator")]
    private async Task<List<ObservableDecomposedMember>> DecomposeMembersAsync(ObservableDecomposedObject decomposedObject)
    {
        var options = CreateDecomposingOptions();
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

    private ObservableDecomposedObject GenerateRandomObject(Faker faker)
    {
        var options = CreateDecomposingOptions();
        object item = faker.Random.Int(0, 100) switch
        {
            < 10 => faker.Random.Int(0, 100),
            < 20 => faker.Random.Bool(),
            < 30 => faker.Random.Uuid(),
            < 40 => faker.Random.Hexadecimal(),
            < 50 => faker.Date.Future(),
            < 60 => faker.Internet.Url(),
            < 70 => faker.Internet.Email(),
            < 80 => Color.FromArgb(faker.Random.Byte(), faker.Random.Byte(), faker.Random.Byte(), faker.Random.Byte()),
            < 90 => faker.Random.Double(0, 100),
            _ => faker.Lorem.Word()
        };

        return DecompositionResultMapper.Convert(LookupComposer.DecomposeObject(item, options));
    }

    private DecomposeOptions CreateDecomposingOptions()
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