using System.Windows;
using RevitLookup.UI.Playground.Client.Views;

namespace RevitLookup.UI.Playground;

public sealed partial class App
{
    private void OnStartup(object sender, StartupEventArgs e)
    {
        var view = Host.CreateScope<PlaygroundView>();
        view.ShowDialog();
    }
}