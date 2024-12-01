using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JetBrains.Annotations;
using RevitLookup.UI.Framework.Views.AboutProgram;
using RevitLookup.UI.Framework.Views.Dashboard;
using RevitLookup.UI.Framework.Views.Settings;
using RevitLookup.UI.Framework.Views.Summary;
using RevitLookup.UI.Playground.Client.Controls;

namespace RevitLookup.UI.Playground.Client.ViewModels.Pages;

[UsedImplicitly]
public sealed partial class PagesViewModel : ObservableObject
{
    [RelayCommand]
    private void ShowDashboardPage()
    {
        var viewer = Host.CreateScope<PageViewer>();
        viewer.SizeToContent = SizeToContent.Width;
        viewer.Height = 850;
        viewer.ShowPage<DashboardPage>();
    }

    [RelayCommand]
    private void ShowSnoopSummaryPage()
    {
        var viewer = Host.CreateScope<PageViewer>();
        viewer.SizeToContent = SizeToContent.Manual;
        viewer.Height = 500;
        viewer.Width = 900;
        viewer.ShowPage<SnoopSummaryPage>();
    }

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