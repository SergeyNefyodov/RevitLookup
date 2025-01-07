using System.Collections.ObjectModel;
using System.Windows.Media;
using Bogus;
using CommunityToolkit.Mvvm.ComponentModel;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using RevitLookup.Abstractions.ObservableModels.Decomposition;
using RevitLookup.Abstractions.Services.Application;
using RevitLookup.Abstractions.Services.Decomposition;
using RevitLookup.Abstractions.Services.Presentation;
using RevitLookup.Abstractions.ViewModels.Decomposition;
using RevitLookup.UI.Framework.Extensions;
using RevitLookup.UI.Framework.Views.Decomposition;
#if NETFRAMEWORK
using RevitLookup.UI.Framework.Extensions;
#endif

namespace RevitLookup.UI.Playground.Mockups.ViewModels.Decomposition;

[UsedImplicitly]
public sealed partial class MockEventsSummaryViewModel(
    IWindowIntercomService intercomService,
    INotificationService notificationService,
    IDecompositionService decompositionService,
    ILogger<MockDecompositionSummaryViewModel> logger)
    : ObservableObject, IEventsSummaryViewModel
{
    private CancellationTokenSource? _cancellationTokenSource;

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

    public async Task OnNavigatedToAsync()
    {
        _cancellationTokenSource = new CancellationTokenSource();

        var faker = new Faker();
        var cancellationToken = _cancellationTokenSource.Token;
        while (!cancellationToken.IsCancellationRequested)
        {
            var decomposedObject = await GenerateRandomObjectAsync(faker);
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

        decomposedObject.Members = await decompositionService.DecomposeMembersAsync(decomposedObject);
    }

    private async Task<ObservableDecomposedObject> GenerateRandomObjectAsync(Faker faker)
    {
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

        return await decompositionService.DecomposeAsync(item);
    }
}