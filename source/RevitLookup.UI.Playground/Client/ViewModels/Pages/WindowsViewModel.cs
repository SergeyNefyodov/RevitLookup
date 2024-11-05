using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JetBrains.Annotations;

namespace RevitLookup.UI.Playground.Client.ViewModels.Pages;

[UsedImplicitly]
public sealed partial class WindowsViewModel : ObservableObject
{
    [RelayCommand]
    private void ShowUpdater()
    {
        // var view = Host.CreateScope<UpdaterView>();
        // view.ShowDialog();
    }
}