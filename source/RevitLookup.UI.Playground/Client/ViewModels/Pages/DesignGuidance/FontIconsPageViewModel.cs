using System.IO;
using System.Reflection;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using JetBrains.Annotations;
using RevitLookup.UI.Playground.Client.Models;

namespace RevitLookup.UI.Playground.Client.ViewModels.Pages.DesignGuidance;

[UsedImplicitly]
public partial class FontIconsPageViewModel : ObservableObject
{
    [ObservableProperty] private List<FontIconData> _icons;
    [ObservableProperty] private List<FontIconData> _filteredIcons = [];
    [ObservableProperty] private FontIconData? _selectedIcon;
    [ObservableProperty] private string _searchText = string.Empty;

    public FontIconsPageViewModel()
    {
        var jsonText = ReadIconData();
        Icons = JsonSerializer.Deserialize<List<FontIconData>>(jsonText)!
            .OrderBy(data => data.Name)
            .ToList();

        SelectedIcon = _icons.FirstOrDefault();
    }

    private static string ReadIconData()
    {
        const string resourceName = "RevitLookup.UI.Playground.Client.Models.FontIcons.json";

        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(resourceName)!;
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    partial void OnIconsChanged(List<FontIconData> value)
    {
        FilteredIcons = value;
    }

    async partial void OnSearchTextChanged(string value)
    {
        FilteredIcons = await Task.Run(() =>
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Icons;
            }

            var formattedText = value.Trim();
            var results = new List<FontIconData>();

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var setData in Icons)
            {
                if (setData.Name.Contains(formattedText, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(setData);
                }
            }

            return results;
        });
    }
}