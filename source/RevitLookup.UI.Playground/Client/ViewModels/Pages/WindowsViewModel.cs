using System.Reflection;
using System.Windows.Media;
using Bogus;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JetBrains.Annotations;
using RevitLookup.Abstractions.Services;
using RevitLookup.UI.Framework.Views.Dashboard;
using RevitLookup.UI.Framework.Views.Summary;

namespace RevitLookup.UI.Playground.Client.ViewModels.Pages;

[UsedImplicitly]
public sealed partial class WindowsViewModel : ObservableObject
{
    [RelayCommand]
    private void ShowRevitLookupWindow()
    {
        Host.GetService<IRevitLookupUiService>()
            .Show<DashboardPage>();
    }

    [RelayCommand]
    private void DecomposeColors()
    {
        var faker = new Faker();

        var colors = new List<Color>();
        for (var i = 0; i < 200; i++)
        {
            colors.Add(Color.FromArgb(
                faker.Random.Byte(),
                faker.Random.Byte(),
                faker.Random.Byte(),
                faker.Random.Byte()
            ));
        }

        Host.GetService<IRevitLookupUiService>()
            .Decompose(colors)
            .Show<SnoopSummaryPage>();
    }

    [RelayCommand]
    private void DecomposeText()
    {
        var faker = new Faker();

        var strings = new List<string>();
        for (var i = 0; i < 1000; i++)
        {
            strings.Add(faker.Lorem.Sentence(69));
        }

        Host.GetService<IRevitLookupUiService>()
            .Decompose(strings)
            .Show<SnoopSummaryPage>();
    }

    [RelayCommand]
    private void DecomposeAssembly()
    {
        var assembly = Assembly.GetExecutingAssembly();
        Host.GetService<IRevitLookupUiService>()
            .Decompose(assembly.GetTypes())
            .Show<SnoopSummaryPage>();
    }
}