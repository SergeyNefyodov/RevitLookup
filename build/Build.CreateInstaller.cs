using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
                var exeFile = Directory.EnumerateFiles(installer.Directory, exePattern, SearchOption.AllDirectories)
                    .FirstOrDefault()
                    .NotNull($"No installer file was found for the project: {installer.Name}");

                var directories = Directory.GetDirectories(project.Directory, "* Release *", SearchOption.AllDirectories);
                Assert.NotEmpty(directories, "No files were found to create an installer");

                foreach (var directory in directories)
                {
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = exeFile,
                        Arguments = directory.DoubleQuoteIfNeeded(),
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };

                    var process = Process.Start(startInfo)!;

                    RedirectStream(process.StandardOutput, LogEventLevel.Information);
                    RedirectStream(process.StandardError, LogEventLevel.Error);

                    process.WaitForExit();
                    if (process.ExitCode != 0) throw new InvalidOperationException($"The installer creation failed with ExitCode {process.ExitCode}");
                }
            }
        });

    [SuppressMessage("ReSharper", "TemplateIsNotCompileTimeConstantProblem")]
    void RedirectStream(StreamReader reader, LogEventLevel eventLevel)
    {
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (line is null) continue;

            var matches = StreamRegex.Matches(line);
            if (matches.Count > 0)
            {
                var parameters = matches
                    .Select(match => match.Value.Substring(1, match.Value.Length - 2))
                    .Cast<object>()
                    .ToArray();

                var messageTemplate = StreamRegex.Replace(line, match => $"{{Parameter{match.Index}}}");
                Log.Write(eventLevel, messageTemplate, parameters);
            }
            else
            {
                Log.Debug(line);
            }
        }
    }
}