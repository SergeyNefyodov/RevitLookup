using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using RevitLookup.UI.Framework.Views.Dashboard;
using RevitLookup.UI.Framework.Views.Windows;
using Wpf.Ui;

namespace RevitLookup.UI.Playground.Client.ViewModels.Pages;

[UsedImplicitly]
public sealed partial class WindowsViewModel : ObservableObject
{
    [RelayCommand]
    private void ShowRevitLookupWindow()
    {
        var scopeFactory = Host.GetService<IServiceScopeFactory>();
        var scope = scopeFactory.CreateScope();

        var view = scope.ServiceProvider.GetRequiredService<RevitLookupView>();
        var navigationService = scope.ServiceProvider.GetRequiredService<INavigationService>();

        view.Closed += (_, _) => scope.Dispose();
        view.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        view.Show();
        navigationService.Navigate(typeof(DashboardPage));
    }
}