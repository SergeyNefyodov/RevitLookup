using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JetBrains.Annotations;
using RevitLookup.UI.Playground.Client.Views.Pages;
using RevitLookup.UI.Playground.Client.Views.Pages.DesignGuidance;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace RevitLookup.UI.Playground.Client.ViewModels;

[UsedImplicitly]
public sealed class PlaygroundViewModel : ObservableObject
{
    public List<object> MenuItems { get; } =
    [
        new NavigationViewItem("Home", SymbolRegular.Home24, typeof(DashboardPage)),
        new NavigationViewItem
        {
            Content = "Design guidance",
            Icon = new SymbolIcon(SymbolRegular.DesignIdeas24, 18),
            MenuItemsSource = new object[]
            {
                new NavigationViewItem("Typography", SymbolRegular.TextFont24, typeof(TypographyPage)),
                // new NavigationViewItem("Colors", SymbolRegular.Color24, typeof(DashboardPage)),
                new NavigationViewItem("Segoe icons", SymbolRegular.Diversity24, typeof(FontIconsPage)),
                new NavigationViewItem("Fluent icons", SymbolRegular.Diversity24, typeof(SymbolIconsPage)),
            }
        },
        // new NavigationViewItemSeparator(),
    ];

    public List<object> FooterItems { get; } =
    [
        new NavigationViewItem("Switch theme", SymbolRegular.DarkTheme24, null!) { Command = new RelayCommand(SwitchApplicationTheme) }
    ];

    private static void SwitchApplicationTheme()
    {
        var applicationTheme = ApplicationThemeManager.GetAppTheme();
        ApplicationThemeManager.Apply(applicationTheme == ApplicationTheme.Light ? ApplicationTheme.Dark : ApplicationTheme.Light);
    }
}