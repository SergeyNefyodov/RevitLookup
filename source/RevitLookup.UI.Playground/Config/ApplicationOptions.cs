using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RevitLookup.Abstractions.Options;
using RevitLookup.Common.Utils;

namespace RevitLookup.UI.Playground.Config;

public static class ApplicationOptions
{
    public static void AddApplicationOptions(this IServiceCollection services)
    {
        services.Configure<ConsoleLifetimeOptions>(options => options.SuppressStatusMessages = true);
    }

    public static void AddFolderOptions(this IServiceCollection services)
    {
        var rootPath = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory!.FullName;
        services.Configure<FoldersOptions>(options =>
        {
            options.RootFolder = rootPath;
            options.ConfigFolder = Path.Combine(rootPath, "Config");
            options.DownloadsFolder = Path.Combine(rootPath, "Downloads");
            options.GeneralSettingsPath = Path.Combine(rootPath, "Config", "Settings.cfg");
            options.RenderSettingsPath = Path.Combine(rootPath, "Config", "RenderSettings.cfg");
        });
    }

    public static void AddAssemblyOptions(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var assemblyLocation = assembly.Location;
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        var fileVersion = new Version(FileVersionInfo.GetVersionInfo(assemblyLocation).FileVersion!);

        var targetFrameworkAttribute = assembly.GetCustomAttributes(typeof(TargetFrameworkAttribute), true)
            .Cast<TargetFrameworkAttribute>()
            .First();

        services.Configure<AssemblyOptions>(options =>
        {
            options.Framework = targetFrameworkAttribute.FrameworkDisplayName ?? targetFrameworkAttribute.FrameworkName;
            options.Version = new Version(fileVersion.Major, fileVersion.Minor, fileVersion.Build);
            options.HasAdminAccess = assemblyLocation.StartsWith(appDataPath) || !AccessUtils.CheckWriteAccess(assemblyLocation);
        });
    }
}