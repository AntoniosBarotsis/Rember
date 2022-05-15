using CliFx;
using CliFx.Attributes;
using CliFx.Exceptions;
using CliFx.Infrastructure;
using Optional;
using Rember.Extensions;
using Rember.Models;
using Rember.Util;

namespace Rember.Commands;

[Command(
    "init",
    Description =
        @"Creates a pre-push git hook. 

  By default the hook will both build and test your code unless specified otherwise with the -b and -t flags.
  Alternatively you can point to a config yml file using the -f flag.

  By default, you will be asked whether you want to run builds and tests which is great if you are using the terminal
  but will probably cause issues with GUIs. Be sure to use the -y flag if you are using GUIs."
)]
public class Init : ICommand
{
    private readonly FileLoader _fileLoader = FileLoader.Instance;
    private readonly string _path = Directory.GetCurrentDirectory();

    // These 2 hold the actual values
    private bool _build = true;
    private bool _test = true;

    [CommandOption("build", 'b', Description = "Only enables the Build task (disables Test)")]
    public bool IncludeBuild
    {
        get => _build;
        // ReSharper disable once ValueParameterNotUsed
        set => _test = false;
    }

    [CommandOption("test", 't', Description = "Only enables the Test task (disables Build)")]
    public bool IncludeTest
    {
        get => _test;
        // ReSharper disable once ValueParameterNotUsed
        set => _build = false;
    }

    [CommandOption("file", 'f', Description = "Point to a yml config file",
        Converter = typeof(OptionFilePathConverter))]
    public Option<string> ConfigPath { get; set; }

    [CommandOption("yes", 'y', Description = "Always runs the tasks without asking you.")]
    public bool AlwaysRun { get; set; } = false;

    public ValueTask ExecuteAsync(IConsole console)
    {
        if (!IsGitRepository()) throw new CommandException("Current folder is not a git repository.");

        var buildTool = BuildToolDetector.GetBuildTool(_fileLoader.DirectorySearch(_path));

        // Get hook contents from either yml config or the passed args
        var res = ConfigPath.Match(
            InitFromYml,
            () => buildTool.Match(
                AddTasks,
                () => throw new CommandException("Build tool not recognized")
            )
        )!;

        res.ThrowCmdExceptionIfEmpty();

        HookFileFacade.Instance.WriteToFile(res);
        HookFileFacade.Instance.SaveChanges();

        return default;
    }

    /// <summary>
    ///     Adds build/test depending on user args (not config file)
    /// </summary>
    /// <param name="buildTool">The detected build tool</param>
    /// <returns>The hook contents</returns>
    private string AddTasks(BuildTool buildTool)
    {
        var hook = new HookUtils(buildTool);
        var (header, b1) = hook.GenerateBuildScript(_build).SetAlwaysRunTo(AlwaysRun, "Build");
        var (_, b2) = hook.GenerateTestScript(_test).SetAlwaysRunTo(AlwaysRun, "Test");

        return $"{header}\n{b1}\n{b2}";
    }

    private static bool IsGitRepository()
    {
        var currentDirectory = Directory.GetCurrentDirectory();

        return Directory
            .GetDirectories(currentDirectory, ".git", SearchOption.TopDirectoryOnly)
            .Length > 0;
    }

    /// <summary>
    ///     Reads an applies config from yml file
    /// </summary>
    /// <param name="p">Path to yml file</param>
    /// <returns>The hook contents</returns>
    /// <exception cref="CommandException">If the build tool is invalid or if something generally went wrong.</exception>
    private string InitFromYml(string p)
    {
        EnsureValidYml(p);

        var ymlContents = string.Join("\n", File.ReadLines(p));
        var config = YmlStuff.Deserialize(ymlContents);

        var buildTool = BuildTool
            .SupportedBuildTools()
            .FirstOrDefault(bt => bt.Name == config.BuildToolName)!
            .SomeNotNull();

        return buildTool.Match(
            bt =>
            {
                var hook = new HookUtils(bt);
                var body = "";
                var header = "";

                foreach (var task in config.Tasks)
                {
                    var (h, b) =
                        HookUtils
                            .GenerateAnyScript(task.Name, task.Command, task.IsEnabled, task.OutputEnabled)
                            .SetAlwaysRunTo(task.AlwaysRun, task.Name);

                    header = h;
                    body += HookUtils.ToggleAutomaticRun($"{b}\n", task.Name, AlwaysRun);
                }

                if (header.Length == 0 || body.Length == 0)
                    throw new CommandException("Something went wrong while parsing the config file");

                return $"{header}\n{body}";
            },
            () => throw new CommandException($"Invalid build tool {config.BuildToolName}")
        );
    }

    /// <summary>
    ///     Ensures that the file exists and is a yml/yaml file.
    /// </summary>
    /// <param name="p">Path to file</param>
    /// <exception cref="CommandException">If the file is invalid</exception>
    private static void EnsureValidYml(string p)
    {
        var exists = File.Exists(p);

        if (!exists)
            throw new CommandException($"File {p} does not exist");
        if (!p.EndsWith(".yml") && !p.EndsWith(".yaml"))
            throw new CommandException($"File {p} is not a yml file");
    }
}