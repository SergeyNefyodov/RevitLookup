using RevitLookup.UI.Playground.Client.ViewModels.Pages;

namespace RevitLookup.UI.Playground.Client.Views.Pages;

public sealed partial class DashboardPage
{
    public DashboardPage(DashboardViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}