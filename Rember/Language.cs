using Rember.Tasks;

namespace Rember;

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

    public static string[] Ignored { get; } =
    {
        "idea", "vs", "vscode",
        "debug", "release", "build",
        "bin", "obj", "out", "git",
        "node_modules"
    };

    public static Language[] SupportedLanguages { get; } =
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