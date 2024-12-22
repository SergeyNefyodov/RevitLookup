using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using RevitLookup.Abstractions.Services.Presentation;
using RevitLookup.UI.Framework.Controls.Automation;
using Wpf.Ui;

namespace RevitLookup.UI.Playground.Client.Controls;

public sealed partial class PageViewer
{
    private readonly IServiceProvider _serviceProvider;

    public PageViewer(
        IServiceProvider serviceProvider,
        ISnackbarService snackbarService,
        IContentDialogService dialogService,
        IWindowIntercomService intercomService)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        intercomService.SetHost(this);
        dialogService.SetDialogHost(RootContentDialog);
        snackbarService.SetSnackbarPresenter(RootSnackbar);
    }

    public bool? ShowPage<T>() where T : Page
    {
        var page = _serviceProvider.GetRequiredService<T>();
        Viewer.Navigate(page);

        if (WindowStartupLocation == WindowStartupLocation.CenterScreen) Viewer.SizeChanged += OnViewerFrameResized;
        return ShowDialog();
    }

    public void RunService<T>(Action<T> handler) where T : class
    {
        var service = _serviceProvider.GetRequiredService<T>();
        handler.Invoke(service);
    }

    private void OnViewerFrameResized(object sender, SizeChangedEventArgs args)
    {
        if (args.PreviousSize.Height == 0 || args.PreviousSize.Width == 0) return;

        var self = (Frame) sender;
        self.SizeChanged -= OnViewerFrameResized;

        //Move the owner to the screen center after navigation
        if (SizeToContent is SizeToContent.WidthAndHeight or SizeToContent.Width)
        {
            Left -= (ActualWidth - MinWidth) / 2;
        }

        if (SizeToContent is SizeToContent.WidthAndHeight or SizeToContent.Height)
        {
            Top -= (ActualHeight - MinHeight) / 2;
        }
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new NoAutomationWindowPeer(this);
    }
}