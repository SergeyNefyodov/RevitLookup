using RevitLookup.UI.Playground.Client.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;

namespace RevitLookup.UI.Playground.Client.Views.Pages;

public sealed partial class DialogsPage : INavigableView<DialogsViewModel>
{
    public DialogsPage(DialogsViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;
        InitializeComponent();
    }

    public DialogsViewModel ViewModel { get; }
}