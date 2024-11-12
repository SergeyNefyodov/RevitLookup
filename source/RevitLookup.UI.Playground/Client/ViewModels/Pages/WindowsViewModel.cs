using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JetBrains.Annotations;
using RevitLookup.UI.Framework.Views.Windows;

namespace RevitLookup.UI.Playground.Client.ViewModels.Pages;

[UsedImplicitly]
public sealed partial class WindowsViewModel : ObservableObject
{
    [RelayCommand]
    private void ShowRevitLookupWindow()
    {
        var view = Host.CreateScope<RevitLookupView>();
        view.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        view.Show();
    }
}