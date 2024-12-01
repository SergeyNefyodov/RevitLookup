using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Wpf.Ui.Controls;

namespace RevitLookup.UI.Playground.Client.Services;

public static class ViewsRegistrationExtensions
{
    public static void RegisterViews(this IServiceCollection services)
    {
        services.Scan(selector => selector.FromAssemblyOf<Framework.App>()
            .AddClasses(filter => filter.AssignableTo<FluentWindow>()).AsSelf().WithScopedLifetime()
            .AddClasses(filter => filter.AssignableTo<Page>()).AsSelf().WithScopedLifetime()
            .AddClasses(filter => filter.AssignableTo<ContentDialog>()).AsSelf().WithTransientLifetime());

        services.Scan(selector => selector.FromCallingAssembly()
            .AddClasses(filter => filter.AssignableTo<FluentWindow>()).AsSelf().WithScopedLifetime()
            .AddClasses(filter => filter.AssignableTo<Page>()).AsSelf().WithScopedLifetime());
    }
}