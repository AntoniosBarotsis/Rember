namespace Rember.Models;

/// <summary>
///     Represents any programming language. Includes data associated with it that is later used to infer the language
///     such as file extensions, special files and a list of build tools.
/// </summary>
public class Language
{
    private Language(string name, string[] extensions, string[] associatedFiles, BuildTool[] buildTools)
    {
        Name = name;
        Extensions = extensions;
        AssociatedFiles = associatedFiles;
        BuildTools = buildTools;
    }

    public string Name { get; }
    public string[] Extensions { get; }
    public string[] AssociatedFiles { get; }
    public BuildTool[] BuildTools { get; }

    /// <summary>
    ///     A list of files/directories that should be ignored to save time.
    ///     Don't quite remember why I decided to remove the dot from the filenames
    ///     and also do that while checking them in order to ignore them but yes.
    /// </summary>
    public static IEnumerable<string> Ignored { get; } = new[]
    {
        "idea", "vs", "vscode",
        "debug", "release", "build",
        "bin", "obj", "out", "git",
        "node_modules"
    };

    /// <summary>
    ///     List of supported languages. Javascript may or may not also include Typescript, who knows.
    /// </summary>
    public static IEnumerable<Language> SupportedLanguages { get; } = new[]
    {
        Java, Csharp, Javascript, Scala
    };

    private static Language Java => new(
        "Java",
        new[] { "java" },
        Array.Empty<string>(),
        new[] { BuildTool.Gradle, BuildTool.Maven }
    );

    private static Language Csharp => new(
        "C#",
        new[] { "cs", "csproj", "sln" },
        new[] { "Program.cs", "Startup.cs" },
        new[] { BuildTool.Dotnet }
    );

    private static Language Javascript => new(
        "JavaScript",
        new[] { "js", "ts" },
        new[] { "package.json" },
        new[] { BuildTool.Npm, BuildTool.Yarn }
    );

    private static Language Scala => new(
        "Scala",
        new[] { "scala" },
        Array.Empty<string>(),
        new[] { BuildTool.Sbt }
    );
}