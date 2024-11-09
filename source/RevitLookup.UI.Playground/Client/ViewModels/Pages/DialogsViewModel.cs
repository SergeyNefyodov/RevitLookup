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
    private async Task ShowOpenSourceDialogAsync()
    {
        var dialog = serviceProvider.GetRequiredService<OpenSourceDialog>();
        await dialog.ShowAsync();
    }

    [RelayCommand]
    private async Task ShowSearchElementsDialogAsync()
    {
        var dialog = serviceProvider.GetRequiredService<SearchElementsDialog>();
        await dialog.ShowAsync();
    }

    [RelayCommand]
    private async Task ShowModulesDialogAsync()
    {
        var dialog = serviceProvider.GetRequiredService<ModulesDialog>();
        await dialog.ShowAsync();
    }

    [RelayCommand]
    private async Task ShowParametersDialogAsync()
    {
        var dialog = serviceProvider.GetRequiredService<UnitsDialog>();
        await dialog.ShowParametersDialogAsync();
    }

    [RelayCommand]
    private async Task ShowCategoriesDialogAsync()
    {
        var dialog = serviceProvider.GetRequiredService<UnitsDialog>();
        await dialog.ShowCategoriesDialogAsync();
    }

    [RelayCommand]
    private async Task ShowForgeSchemaDialogAsync()
    {
        var dialog = serviceProvider.GetRequiredService<UnitsDialog>();
        await dialog.ShowForgeSchemaDialogAsync();
    }
}