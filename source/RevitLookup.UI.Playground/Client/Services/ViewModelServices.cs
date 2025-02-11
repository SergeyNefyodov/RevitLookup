using Microsoft.Extensions.DependencyInjection;

namespace RevitLookup.UI.Playground.Client.Services;

public static class ViewModelServices
{
    public static void RegisterViewModels(this IServiceCollection services)
    {
        services.Scan(selector => selector.FromAssemblyOf<App>()
            .AddClasses(filter => filter.InNamespaces("RevitLookup.UI.Playground.Client.ViewModels"))
            .AsSelf()
            .WithScopedLifetime()
            .AddClasses(filter => filter.NotInNamespaces("RevitLookup.UI.Playground.Client.ViewModels").Where(type => type.Name.EndsWith("ViewModel")))
            .AsImplementedInterfaces(type => type.Name.EndsWith("ViewModel"))
            .WithScopedLifetime());
    }
}