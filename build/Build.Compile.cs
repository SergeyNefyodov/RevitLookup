using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

sealed partial class Build
{
    Target Compile => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            foreach (var configuration in GlobBuildConfigurations())
            {
                if (!AssemblyVersionsMap.TryGetValue(configuration, out var version)) version = "1.0.0";

                DotNetBuild(settings => settings
                    .SetConfiguration(configuration)
                    .SetVersion(version)
                    .SetVerbosity(DotNetVerbosity.minimal));
            }
        });
}