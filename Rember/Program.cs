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
var remainingArgs = args.Skip(1).ToList();
var res = args[0].Trim() switch
{
    "init" => Init(remainingArgs),
    "forgor" => Clear(),
    "logs" => ToggleOutput(remainingArgs),
    "create" => CreateCommand(remainingArgs),
    "enable" => EnableCommand(remainingArgs.First()),
    "disable" => DisableCommand(remainingArgs.First()),
    "save" => Save(),
    "restore" => Restore(),
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
// TODO Optimize
string Init(List<string> args)
{
    if (!IsGitRepository()) return "Current folder is not a git repository.";

    var buildTool = GetBuildTool();
    
    if (buildTool is null) return "Build tool not recognized";

    Console.WriteLine($"Recognized build tool: {buildTool.Name}");

    var generator = 
        new PreActionGenerator(buildTool, Type.Commit)
            .AddBuildScript()
            .AddTestScript();

    generator.WriteToFile();

    if (args.Count == 0 || args[0] == "")
    {
        return "Rember initialized";
    } 
    
    switch (args[0])
    {
        case "build" or "-b":
            DisableCommand("test", generator.Text);
            break;
        case "test" or "-t":
            DisableCommand("build", generator.Text);
            break;
        default:
            return $"Invalid option {args[0]}";
    }

    return "Rember initialized";
}

string ToggleOutput(List<string> args)
{
    var editor = new ActionEditor(Type.Commit);

    var result = args[0].Trim() switch
    {
        "enable" => editor.StageEdit(EditType.OutputEnable),
        "disable" => editor.StageEdit(EditType.OutputDisable),
        _ => "Invalid switch, can be either \"logs enable\" or \"logs disable\"."
    };

    editor.ApplyEdits();

    return result;
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

string EnableCommand(string commandName)
{
    var editor = new ActionEditor(Type.Commit);
    var result = editor.StageEdit(EditType.TaskEnable, commandName);
    editor.ApplyEdits();
    return result;
}

string DisableCommand(string commandName, string text = "")
{
    var editor = text == "" ? 
        new ActionEditor(Type.Commit) :
        new ActionEditor(Type.Commit, text);
    
    var result = editor.StageEdit(EditType.TaskDisable, commandName);
    editor.ApplyEdits();
    return result;
}

string Save()
{
    var path = Directory.GetCurrentDirectory() + "/.git/hooks/pre-commit";

    if (!File.Exists(path))
    {
        return "File is missing, could not save.";
    }
    
    var text = new HookAccessor(path).Text;
    
    using var sw = new StreamWriter(File.Create(path + ".txt"));
    sw.Write(text);
    sw.Close();
    
    return "Hooks saved!";
}

string Restore()
{
    var path = Directory.GetCurrentDirectory() + "/.git/hooks/pre-commit";
    
    if (!File.Exists(path + ".txt"))
    {
        return "Save-file is missing, could not restore.";
    }
        
    using var sr = new StreamReader(File.OpenRead(path + ".txt"));
    var text = sr.ReadToEnd();
    sr.Close();
    File.Delete(path + ".txt");

    new HookAccessor(path, text)
        .SaveChanges();

    return "Hooks restored!";
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
  - rember init [-b | -t]: Initializes a pre commit and push hook that builds and tests. The `b` and `t` flags specify 
    will only create the build or test task respectively. Either can be added with `enable` later.
  - rember forgor: Removes said hooks.
  - rember logs enable: Enables the output of your builds and tests
  - rember logs disable: Disables the output of your builds and tests
  - rember create TaskName TaskCommand: Creates a custom command (TaskName should be a valid variable name in bash) [BUGGY]
  - rember enable TaskName: Enables the given task
  - rember disable TaskName: Disables the given task";
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