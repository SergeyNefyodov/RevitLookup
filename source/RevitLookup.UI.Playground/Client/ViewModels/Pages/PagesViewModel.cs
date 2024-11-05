using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JetBrains.Annotations;
using RevitLookup.UI.Framework.Views.AboutProgram;
using RevitLookup.UI.Framework.Views.Settings;
using RevitLookup.UI.Playground.Client.Controls;

namespace RevitLookup.UI.Playground.Client.ViewModels.Pages;

[UsedImplicitly]
public sealed partial class PagesViewModel : ObservableObject
{
    [RelayCommand]
    private void ShowSettingsPage()
    {
        var viewer = Host.CreateScope<PageViewer>();
        viewer.ShowPage<SettingsPage>();
    }
    
    [RelayCommand]
    private void ShowAboutPage()
    {
        var viewer = Host.CreateScope<PageViewer>();
        viewer.ShowPage<AboutPage>();
    }
}