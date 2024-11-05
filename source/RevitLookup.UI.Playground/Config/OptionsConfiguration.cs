using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace RevitLookup.UI.Playground.Config;

public static class OptionsConfiguration
{
    /// <summary>
    ///    Add global JsonSerialization configuration/>
    /// </summary>
    public static void AddSerializerOptions(this IServiceCollection services)
    {
        services.Configure<JsonSerializerOptions>(options =>
        {
#if DEBUG
            options.WriteIndented = true;
#endif
            options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });
    }
}