using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

sealed partial class Build
{
    Target Test => _ => _
        .Executes(() =>
        {
            DotNetTest(settings => settings
                .SetConfiguration("Release Engine")
                .SetVerbosity(DotNetVerbosity.minimal));
        });
}