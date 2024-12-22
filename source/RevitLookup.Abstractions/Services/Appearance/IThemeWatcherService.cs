using System.Windows;

namespace RevitLookup.Abstractions.Services.Appearance;

public interface IThemeWatcherService
{
    void Initialize();
    void Watch();
    void Watch(FrameworkElement frameworkElement);
    void Unwatch();
}