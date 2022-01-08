namespace Rember;

public class BuildTool
{
    // TODO Investigate if npm vs yarn are distinguishable or if reading package.json is needed
    // TODO if so add some additional thing that reads into the file and determines that.   

    private BuildTool(string name, string[] associatedFiles, string build, string test)
    {
        Name = name;
        AssociatedFiles = associatedFiles;
        Build = build;
        Test = test;
    }

    public string Name { get; }
    public string[] AssociatedFiles { get; }
    public string Build { get; }
    public string Test { get; }

    public static BuildTool[] SupportedBuildTools => new[] { Gradle, Maven, Dotnet, Npm, Yarn, Sbt };

    public static BuildTool Gradle => new(
        "Gradle",
        new[] { "gradle.settings", "build.gradle" },
        "gradle build",
        "gradle test"
    );

    public static BuildTool Maven => new(
        "Maven",
        new[] { "pom.xml" },
        "mvn build",
        "mvn test"
    );

    public static BuildTool Dotnet => new(
        "Dotnet",
        Array.Empty<string>(),
        "dotnet build",
        "dotnet test"
    );

    public static BuildTool Npm => new(
        "NPM",
        new[] { "package-lock.json" },
        "npm build",
        "npm test"
    );

    public static BuildTool Yarn => new(
        "Yarn",
        new[] { "yarn.lock" },
        "yarn build",
        "yarn test"
    );

    public static BuildTool Sbt => new(
        "SBT",
        new[] { "build.sbt" },
        "sbt compile --java-home \"$JAVA_HOME\"", // had a weird bug where if I didn't add this flag
        "sbt test --java-home \"$JAVA_HOME\"" // it would use the wrong version, might be only me idk
    );
}