using System.Windows;
using System.Windows.Automation.Peers;
using RevitLookup.Abstractions.Services;
using RevitLookup.UI.Framework.Controls.Automation;
using RevitLookup.UI.Playground.Client.ViewModels;
using RevitLookup.UI.Playground.Client.Views.Pages;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace RevitLookup.UI.Playground.Client.Views;

public sealed partial class PlaygroundView
{
    private readonly INavigationService _navigationService;

    public PlaygroundView(
        PlaygroundViewModel viewModel,
        INavigationService navigationService,
        IContentDialogService dialogService,
        ISnackbarService snackbarService,
        IWindowIntercomService intercomService)
    {
        _navigationService = navigationService;
        DataContext = viewModel;
        InitializeComponent();

        navigationService.SetNavigationControl(NavigationView);
        dialogService.SetDialogHost(RootContentDialog);
        snackbarService.SetSnackbarPresenter(SnackbarPresenter);
        intercomService.SetHost(this);

        Loaded += (sender, _) =>
        {
            var self = (PlaygroundView)sender;
            self._navigationService.Navigate(typeof(DashboardPage));
        };
    }

    private void OnNavigationSelectionChanged(object sender, RoutedEventArgs e)
    {
        if (sender is not NavigationView navigationView) return;

        var onControlsPage = navigationView.SelectedItem?.TargetPageType != typeof(DashboardPage);
        var showHeader = onControlsPage ? Visibility.Visible : Visibility.Collapsed;

        NavigationView.SetCurrentValue(NavigationView.HeaderVisibilityProperty, showHeader);
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new NoAutomationWindowPeer(this);
    }
}