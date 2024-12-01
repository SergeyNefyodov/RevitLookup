using System.Collections;
using System.Windows.Media;
using Bogus;
using CommunityToolkit.Mvvm.ComponentModel;
using JetBrains.Annotations;
using LookupEngine;
using RevitLookup.Abstractions.ObservableModels.Decomposition;
using RevitLookup.Abstractions.ViewModels.Summary;
using RevitLookup.UI.Playground.Mappers;
#if NETFRAMEWORK
using RevitLookup.UI.Framework.Extensions;
#endif

namespace RevitLookup.UI.Playground.ViewModels.Summary;

[UsedImplicitly]
public sealed partial class MockSnoopSummaryViewModel : ObservableObject, ISnoopSummaryViewModel
{
    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private ObservableDecomposedObject? _selectedDecomposedObject;
    [ObservableProperty] private List<ObservableDecomposedObject> _decomposedObjects = [];
    [ObservableProperty] private List<ObservableDecomposedObjectsGroup> _filteredDecomposedObjects = [];
    [ObservableProperty] private List<ObservableDecomposedMember> _members = [];
    [ObservableProperty] private List<ObservableDecomposedMember> _filteredMembers = [];

    public MockSnoopSummaryViewModel()
    {
        var globalFaker = new Faker();
        var strings = new Faker<string>()
            .CustomInstantiator(faker => faker.Lorem.Sentence(400))
            .GenerateBetween(0, 100);

        var integers = Enumerable.Range(0, globalFaker.Random.Int(1, 100))
            .Select(_ => globalFaker.Random.Int())
            .ToList();

        var colors = Enumerable.Range(0, globalFaker.Random.Int(1, 100))
            .Select(_ => Color.FromRgb(globalFaker.Random.Byte(), globalFaker.Random.Byte(), globalFaker.Random.Byte()))
            .ToList();

        var objects = new ArrayList();
        objects.AddRange(strings);
        objects.AddRange(integers);
        objects.AddRange(colors);

        DecomposedObjects = objects
            .Cast<object>()
            .Select(value => LookupComposer.Decompose(value))
            .Select(DecompositionResultMapper.Convert)
            .ToList();
    }

    partial void OnDecomposedObjectsChanged(List<ObservableDecomposedObject> value)
    {
        OnSearchTextChanged(SearchText);
    }

    partial void OnSelectedDecomposedObjectChanged(ObservableDecomposedObject? value)
    {
        if (value is null)
        {
            Members = [];
            return;
        }

        Members = value.Members;
    }

    partial void OnMembersChanged(List<ObservableDecomposedMember> value)
    {
        FilteredMembers = value;
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
        }
        catch
        {
            // ignored
        }
    }

    private List<ObservableDecomposedObjectsGroup> ApplyGrouping(List<ObservableDecomposedObject> objects)
    {
        return objects
            .OrderBy(data => data.Type)
            .ThenBy(data => data.Name)
            .GroupBy(data => data.Type)
            .Select(group => new ObservableDecomposedObjectsGroup
            {
                GroupName = group.Key,
                GroupItems = group.ToList()
            })
            .ToList();
    }
}