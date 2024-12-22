using System.Windows;
using Bogus;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JetBrains.Annotations;
using RevitLookup.Abstractions.Services.Summary;
using RevitLookup.Abstractions.ViewModels.Summary;
using RevitLookup.UI.Framework.Views.AboutProgram;
using RevitLookup.UI.Framework.Views.Dashboard;
using RevitLookup.UI.Framework.Views.Settings;
using RevitLookup.UI.Framework.Views.Summary;
using RevitLookup.UI.Framework.Views.Tools;
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
    private void ShowDecompositionSummaryPage()
    {
        var viewer = Host.CreateScope<PageViewer>();
        viewer.SizeToContent = SizeToContent.Manual;
        viewer.Height = 500;
        viewer.Width = 900;
        viewer.RunService<IVisualDecompositionService>(service =>
        {
            var faker = new Faker();

            var strings = new List<string>();
            for (var i = 0; i < 1000; i++)
            {
                strings.Add(faker.Lorem.Sentence(300));
            }

            service.VisualizeDecompositionAsync(strings);
        });

        viewer.ShowPage<DecompositionSummaryPage>();
    }

    [RelayCommand]
    private void ShowEventsSummaryPage()
    {
        var viewer = Host.CreateScope<PageViewer>();
        viewer.SizeToContent = SizeToContent.Manual;
        viewer.Height = 500;
        viewer.Width = 900;
        viewer.RunService<IEventsSummaryViewModel>(service => { service.OnNavigatedToAsync(); });

        viewer.Closing += (sender, _) =>
        {
            var self = (PageViewer) sender!;
            self.RunService<IEventsSummaryViewModel>(service => { service.OnNavigatedFromAsync(); });
        };

        viewer.ShowPage<EventsSummaryPage>();
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

    [RelayCommand]
    private void ShowRevitSettingsPage()
    {
        var viewer = Host.CreateScope<PageViewer>();
        viewer.SizeToContent = SizeToContent.Manual;
        viewer.Height = 850;
        viewer.Width = 550;
        viewer.ShowPage<RevitSettingsPage>();
    }
}