using System.Windows;
using System.Windows.Threading;
using JetBrains.Annotations;
using RevitLookup.Abstractions.Services.Presentation;

namespace RevitLookup.UI.Framework.Services.Presentation;

public sealed class WindowIntercomService : IWindowIntercomService
{
    private Window? _host;
    private static readonly List<Window> SharedWindows = [];

    public void SetHost(Window host)
    {
        _host = host;
    }

    public void SetSharedHost(Window host)
    {
        SetHost(host);
        SharedWindows.Add(host);
        host.Closed += OnHostDisconnected;
    }

    private static void OnHostDisconnected(object? sender, EventArgs args)
    {
        var self = (Window) sender!;
        self.Closed -= OnHostDisconnected;

        SharedWindows.Remove(self);
    }

    public List<Window> OpenedWindows => SharedWindows;

    [Pure]
    public Window GetHost()
    {
        if (_host is null) throw new InvalidOperationException("The Host was never set.");
        return _host;
    }

    public Dispatcher Dispatcher
    {
        get
        {
            if (_host is null) throw new InvalidOperationException("The Host was never set.");
            return _host.Dispatcher;
        }
    }
}