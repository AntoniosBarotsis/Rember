using System.Reflection;
using Rember;
using Rember.Actions;
using Rember.FileStuff;
using Rember.Tasks;
using Type = Rember.Actions.Type;

if (args.Length == 0)
{
    Console.WriteLine(Description());
    return;
}

// Get the files in the project directory
var fileLoader = FileLoader.Instance;
var files = fileLoader.DirectorySearch(Directory.GetCurrentDirectory());

// Main switch
var res = args[0].Trim() switch
{
    "init" => Init(args.Skip(1).ToList()),
    "forgor" => Clear(),
    "logs" => ToggleOutput(args.Skip(1).ToList()),
    "create" => CreateCommand(args.Skip(1).ToList()),
    "-h" or "--help" => Description(),
    _ => InvalidArgument(args[0])
};

Console.WriteLine(res);

BuildTool? GetBuildTool(bool log = true)
{
    var lang = LanguageDetector.DetectLanguage(files.Value);

    if (lang is null) return null;
    
    foreach (var langBuildTool in lang.BuildTools) Console.WriteLine($"- {langBuildTool.Name}");

    if (!log) return BuildToolDetector.DetectBuildTool(files.Value, lang);
    
    Console.WriteLine($"Language recognized: {lang.Name}\n");
    Console.WriteLine("Supported build tools:");
    Console.WriteLine("\n");

    return BuildToolDetector.DetectBuildTool(files.Value, lang);
}

// TODO Make it so the user can pick both the lang and build tool, right now it happens automatically.
// TODO There's like 50 things that can go wrong in this, add exception handling.
string Init(List<string> args)
{
    if (!IsGitRepository()) return "Current folder is not a git repository.";

    var buildTool = GetBuildTool();
    
    if (buildTool is null) return "Build tool not recognized";

    Console.WriteLine($"Recognized build tool: {buildTool.Name}");

    new PreActionGenerator(buildTool, Type.Commit)
        .AddBuildScript()
        .AddTestScript()
        .WriteToFile();

    return "Rember initialized";
}

string ToggleOutput(List<string> args)
{
    var editor = new ActionEditor(Type.Commit);

    var res = args[0].Trim() switch
    {
        "enable" => editor.StageEdit(EditType.OutputEnable),
        "disable" => editor.StageEdit(EditType.OutputDisable),
        _ => "Invalid switch, can be either \"logs enable\" or \"logs disable\"."
    };

    editor.ApplyEdits();

    return res;
}

string CreateCommand(List<string> args)
{
    if (args.Count < 2 || args[0] is "" || args[1] is "")
        return "Invalid input, should be `rember create [NAME] [COMMAND]`";

    if (args[0].Contains(' '))
        return "Invalid input, name should be one word";

    var customCommand = new CustomTask(args[0], args[1]);
    var buildTool = GetBuildTool(false);
    
    if (buildTool is null) return "Build tool not recognized";
    
    new PreActionGenerator(buildTool, Type.Commit)
        .AddCustom(customCommand)
        .AppendToFile();
    
    
    return "Command Created!";
}

string Clear()
{
    var preCommit = Directory.GetCurrentDirectory() + "/.git/hooks/pre-commit";
    var prePush = Directory.GetCurrentDirectory() + "/.git/hooks/pre-push";

    File.Delete(preCommit);
    File.Delete(prePush);

    return "Hooks removed!";
}

string Description()
{
    var versionString = Assembly.GetEntryAssembly()?
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
        .InformationalVersion;

    return @$"==== Rember v{versionString} ====
Rember is a command line tool that allows you to easily run builds and tests automatically before
committing code and waiting for it to break the pipeline 15 minutes later.

Usage:
  - rember init: Initializes a pre commit and push hook that builds and tests (more flexibility will be added later)
  - rember forgor: Removes said hooks.
  - rember logs enable: Enables the output of your builds and tests
  - rember logs disable: Disables the output of your builds and tests
  - rember create TaskName TaskCommand: Creates a custom command (TaskName should be a valid variable name in bash)";
}

string InvalidArgument(string arg)
{
    return $"Invalid argument {arg}.";
}

bool IsGitRepository()
{
    var currentDirectory = Directory.GetCurrentDirectory();
    return Directory.GetDirectories(currentDirectory, ".git", SearchOption.TopDirectoryOnly)
        .Length > 0;
}