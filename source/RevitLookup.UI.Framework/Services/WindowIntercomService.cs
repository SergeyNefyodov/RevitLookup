using System.Windows;
using System.Windows.Threading;
using RevitLookup.Abstractions.Services;

namespace RevitLookup.UI.Framework.Services;

public sealed class WindowIntercomService : IWindowIntercomService
{
    private Window? _host;

    public Window Host
    {
        get
        {
            if (_host is null) throw new InvalidOperationException("The Host was never set.");
            return _host;
        }
        private set => _host = value;
    }

    public Dispatcher Dispatcher => Host.Dispatcher;

    public void SetHost(Window host)
    {
        Host = host;
    }
}