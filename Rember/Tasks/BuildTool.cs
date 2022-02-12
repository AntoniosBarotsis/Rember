namespace Rember.Tasks;

/// <summary>
///     Represents language specific tools that can be used to run builds and tests for said language such as Maven, NPM
///     Dotnet and more.
/// </summary>
public class BuildTool
{
    private BuildTool(string name, string[] associatedFiles, ConcreteTask build, ConcreteTask test)
    {
        Name = name;
        AssociatedFiles = associatedFiles;
        Build = build;
        Test = test;
    }

    public string Name { get; }
    public string[] AssociatedFiles { get; }
    public ConcreteTask Build { get; }
    public ConcreteTask Test { get; }

    /// <summary>
    /// List of supported build tools. This is later used to detect the build tool used by a project.
    /// </summary>
    public static IEnumerable<BuildTool> SupportedBuildTools => new[] { Gradle, Maven, Dotnet, Npm, Yarn, Sbt };

    public static BuildTool Gradle => new(
        "Gradle",
        new[] { "gradle.settings", "build.gradle" },
        new ConcreteTask("Build", "gradle build"),
        new ConcreteTask("Test", "gradle test")
    );

    public static BuildTool Maven => new(
        "Maven",
        new[] { "pom.xml" },
        new ConcreteTask("Build", "mvn build"),
        new ConcreteTask("Test", "mvn test")
    );

    public static BuildTool Dotnet => new(
        "Dotnet",
        Array.Empty<string>(),
        new ConcreteTask("Build", "dotnet build"),
        new ConcreteTask("Test", "dotnet test")
    );

    public static BuildTool Npm => new(
        "NPM",
        new[] { "package-lock.json" },
        new ConcreteTask("Build", "npm build"),
        new ConcreteTask("Test", "npm test")
    );

    public static BuildTool Yarn => new(
        "Yarn",
        new[] { "yarn.lock" },
        new ConcreteTask("Build", "yarn build"),
        new ConcreteTask("Test", "yarn test")
    );

    // had a weird bug where if I didn't add this flag
    // it would use the wrong version, might be only me idk
    public static BuildTool Sbt => new(
        "SBT",
        new[] { "build.sbt" },
        new ConcreteTask("Build", "sbt compile --java-home \"$JAVA_HOME\""),
        new ConcreteTask("Test", "sbt test --java-home \"$JAVA_HOME\"")
    );
}