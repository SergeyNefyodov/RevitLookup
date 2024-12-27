using System.Windows;
using RevitLookup.Abstractions.Services.Settings;
using RevitLookup.UI.Playground.Client.Views;

namespace RevitLookup.UI.Playground;

public sealed partial class App
{
    private void OnStartup(object sender, StartupEventArgs e)
    {
        Initialize();

        var view = Host.CreateScope<PlaygroundView>();
        view.ShowDialog();
    }

    private static void Initialize()
    {
        var settingsService = Host.GetService<ISettingsService>();
        settingsService.LoadSettings();
    }
}