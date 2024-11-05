using RevitLookup.UI.Playground.Client.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;

namespace RevitLookup.UI.Playground.Client.Views.Pages;

public sealed partial class PagesPage: INavigableView<PagesViewModel>
{
    public PagesPage(PagesViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;
        InitializeComponent();
    }

    public PagesViewModel ViewModel { get; }
}