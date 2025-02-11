using System.Diagnostics.CodeAnalysis;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities;
using Serilog.Events;

sealed partial class Build
{
    Target CreateInstaller => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            foreach (var (installer, project) in InstallersMap)
            {
                Log.Information("Project: {Name}", project.Name);

                var exePattern = $"*{installer.Name}.exe";
                var exeFile = Directory.EnumerateFiles(installer.Directory / "bin", exePattern, SearchOption.AllDirectories)
                    .FirstOrDefault()
                    .NotNull($"No installer file was found for the project: {installer.Name}");

                var directories = Directory.GetDirectories(project.Directory / "bin", "* Release *", SearchOption.AllDirectories);
                Assert.NotEmpty(directories, "No files were found to create an installer");

                foreach (var directory in directories)
                {
                    var process = ProcessTasks.StartProcess(exeFile, directory.DoubleQuoteIfNeeded(), logInvocation: false, logger: InstallerLogger);
                    process.AssertZeroExitCode();
                }
            }
        });

    /// <summary>
    ///     Logs the output of the installer process.
    /// </summary>
    [SuppressMessage("ReSharper", "TemplateIsNotCompileTimeConstantProblem")]
    void InstallerLogger(OutputType outputType, string output)
    {
        if (outputType == OutputType.Err)
        {
            Log.Error(output);
            return;
        }

        var arguments = ArgumentsRegex.Matches(output);
        var logLevel = arguments.Count switch
        {
            0 => LogEventLevel.Debug,
            > 0 when output.Contains("error", StringComparison.OrdinalIgnoreCase) => LogEventLevel.Error,
            _ => LogEventLevel.Information
        };

        if (arguments.Count == 0)
        {
            Log.Write(logLevel, output);
            return;
        }

        var properties = arguments
            .Select(match => match.Value.Substring(1, match.Value.Length - 2))
            .Cast<object>()
            .ToArray();

        var messageTemplate = ArgumentsRegex.Replace(output, match => $"{{Property{match.Index}}}");
        Log.Write(logLevel, messageTemplate, properties);
    }
}