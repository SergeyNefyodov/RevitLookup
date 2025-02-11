using Microsoft.Extensions.DependencyInjection;

namespace RevitLookup.Services;

public static class ViewModelServices
{
    public static void RegisterViewModels(this IServiceCollection services)
    {
        services.Scan(selector => selector.FromAssemblyOf<RevitLookup.Application>()
            .AddClasses(filter => filter.Where(type => type.Name.EndsWith("ViewModel")))
            .AsImplementedInterfaces(type => type.Name.EndsWith("ViewModel"))
            .WithScopedLifetime());
    }
}