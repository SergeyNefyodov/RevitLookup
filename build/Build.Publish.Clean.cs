using Nuke.Common.Tools.Git;

sealed partial class Build
{
    Target CleanFailedRelease => _ => _
        .Unlisted()
        .AssuredAfterFailure()
        .TriggeredBy(PublishGitHub)
        .Requires(() => ReleaseVersion)
        .OnlyWhenDynamic(() => FailedTargets.Contains(PublishGitHub))
        .Executes(() =>
        {
            Log.Information("Cleaning failed GitHub release");
            GitTasks.Git($"push --delete origin {ReleaseVersion}", logInvocation: false);
        });
}