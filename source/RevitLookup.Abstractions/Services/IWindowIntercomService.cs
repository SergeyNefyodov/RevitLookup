using System.Windows;
using System.Windows.Threading;

namespace RevitLookup.Abstractions.Services;

public interface IWindowIntercomService
{
    Dispatcher Dispatcher { get; }
    Window Host { get; }

    void SetHost(Window host);
}