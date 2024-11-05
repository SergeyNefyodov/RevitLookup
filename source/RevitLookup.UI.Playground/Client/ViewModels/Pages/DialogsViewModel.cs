using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using RevitLookup.UI.Framework.Views.AboutProgram;
using RevitLookup.UI.Framework.Views.Tools;

namespace RevitLookup.UI.Playground.Client.ViewModels.Pages;

[UsedImplicitly]
public sealed partial class DialogsViewModel(IServiceProvider serviceProvider) : ObservableObject
{
    [RelayCommand]
    private async Task ShowOpenSourceDialog()
    {
        var dialog = serviceProvider.GetRequiredService<OpenSourceDialog>();
        await dialog.ShowAsync();
    }

    [RelayCommand]
    private async Task ShowSearchElementsDialog()
    {
        var dialog = serviceProvider.GetRequiredService<SearchElementsDialog>();
        await dialog.ShowAsync();
    }

    [RelayCommand]
    private async Task ShowModulesDialog()
    {
        var dialog = serviceProvider.GetRequiredService<ModulesDialog>();
        await dialog.ShowAsync();
    }
}