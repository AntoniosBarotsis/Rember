using System.Reflection;
using Rember;
using Type = Rember.Type;

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
    "-h" or "--help" => Description(),
    _ => InvalidArgument(args[0])
};

Console.WriteLine(res);

// TODO Make it so the user can pick both the lang and build tool, right now it happens automatically.
// TODO There's like 50 things that can go wrong in this, add exception handling.
string Init(List<string> args)
{
    if (!IsGitRepository()) return "Current folder is not a git repository.";

    var lang = LanguageDetector.DetectLanguage(files.Value);

    if (lang is null) return "Language not recognized";

    Console.WriteLine($"Language recognized: {lang.Name}\n");
    Console.WriteLine("Supported build tools:");
    foreach (var langBuildTool in lang.BuildTools) Console.WriteLine($"- {langBuildTool.Name}");

    Console.WriteLine("\n");

    var buildTool = BuildToolDetector.DetectBuildTool(files.Value, lang);

    if (buildTool is null) return "Build tool not recognized";

    Console.WriteLine($"Recognized build tool: {buildTool.Name}");

    new PreActionGenerator(buildTool, Type.Commit)
        .AddBuildScript()
        .AddTestScript()
        .WriteToFile();

    return "Rember initialized";
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
  - rember forgor: Removes said hooks.";
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