using CommunityToolkit.Mvvm.ComponentModel;
using JetBrains.Annotations;
using RevitLookup.UI.Playground.Client.Models;
using Wpf.Ui.Controls;

namespace RevitLookup.UI.Playground.Client.ViewModels.Pages.DesignGuidance;

[UsedImplicitly]
public partial class SymbolIconsPageViewModel : ObservableObject
{
    [ObservableProperty] private List<SymbolIconData> _icons;
    [ObservableProperty] private List<SymbolIconData> _filteredIcons = [];
    [ObservableProperty] private SymbolIconData? _selectedIcon;
    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private bool _useFilledIcons;

    public SymbolIconsPageViewModel()
    {
        var symbols = Enum.GetNames(typeof(SymbolRegular));
        Icons = symbols.Select(SymbolGlyph.Parse)
            .Select(symbol => new SymbolIconData
            {
                Name = symbol.ToString(),
                Icon = symbol,
                Code = ((int)symbol).ToString("X4")
            })
            .OrderBy(data => data.Name)
            .ToList();

        SelectedIcon = _icons.FirstOrDefault();
    }

    partial void OnIconsChanged(List<SymbolIconData> value)
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
            var results = new List<SymbolIconData>();

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