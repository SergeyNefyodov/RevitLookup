using System.IO.Enumeration;
using JetBrains.Annotations;
using Nuke.Common.Git;
using Nuke.Common.ProjectModel;

[PublicAPI]
sealed partial class Build : NukeBuild
{
    string[] Configurations;
    Dictionary<Project, Project> InstallersMap;
    Dictionary<string, string> AssemblyVersionsMap;

    [GitRepository] readonly GitRepository GitRepository;
    [Solution(GenerateProjects = true)] Solution Solution;

    public static int Main() => Execute<Build>(build => build.Compile);

    List<string> GlobBuildConfigurations()
    {
        var configurations = Solution.Configurations
            .Select(pair => pair.Key)
            .Select(config => config.Remove(config.LastIndexOf('|')))
            .Where(config => Configurations.Any(wildcard => FileSystemName.MatchesSimpleExpression(wildcard, config)))
            .ToList();

        Assert.NotEmpty(configurations, $"No solution configurations have been found. Pattern: {string.Join(" | ", Configurations)}");
        return configurations;
    }
}