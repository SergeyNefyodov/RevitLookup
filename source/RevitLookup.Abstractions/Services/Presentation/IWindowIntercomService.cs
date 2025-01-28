using System.Windows;
using System.Windows.Threading;

namespace RevitLookup.Abstractions.Services.Presentation;

public interface IWindowIntercomService
{
    Dispatcher Dispatcher { get; }
    List<Window> OpenedWindows { get; }

    Window GetHost();
    void SetHost(Window host);
    void SetSharedHost(Window host);
}