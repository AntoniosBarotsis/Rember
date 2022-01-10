using Rember.Actions;

namespace Rember.Tasks;

public class BuildTool
{
    // TODO Investigate if npm vs yarn are distinguishable or if reading package.json is needed
    // TODO if so add some additional thing that reads into the file and determines that.   

    private BuildTool(string name, string[] associatedFiles, TemplateTask build, TemplateTask test)
    {
        Name = name;
        AssociatedFiles = associatedFiles;
        Build = build;
        Test = test;
    }

    public string Name { get; }
    public string[] AssociatedFiles { get; }
    public TemplateTask Build { get; }
    public TemplateTask Test { get; }

    public static BuildTool[] SupportedBuildTools => new[] { Gradle, Maven, Dotnet, Npm, Yarn, Sbt };

    public static BuildTool Gradle => new(
        "Gradle",
        new[] { "gradle.settings", "build.gradle" }, 
        new TemplateTask("Build", "gradle build"), 
        new TemplateTask("Test", "gradle test")
    );

    public static BuildTool Maven => new(
        "Maven",
        new[] { "pom.xml" },
        new TemplateTask("Build", "mvn build"), 
        new TemplateTask("Test", "mvn test")
    );

    public static BuildTool Dotnet => new(
        "Dotnet",
        Array.Empty<string>(),
        new TemplateTask("Build", "dotnet build"), 
        new TemplateTask("Test", "dotnet test")
    );

    public static BuildTool Npm => new(
        "NPM",
        new[] { "package-lock.json" },
        new TemplateTask("Build", "npm build"), 
        new TemplateTask("Test", "npm test")
    );

    public static BuildTool Yarn => new(
        "Yarn",
        new[] { "yarn.lock" },
        new TemplateTask("Build", "yarn build"), 
        new TemplateTask("Test", "yarn test")
    );

    // had a weird bug where if I didn't add this flag
    // it would use the wrong version, might be only me idk
    public static BuildTool Sbt => new(
        "SBT",
        new[] { "build.sbt" },
        new TemplateTask("Build", "sbt compile --java-home \"$JAVA_HOME\""), 
        new TemplateTask("Test", "sbt test --java-home \"$JAVA_HOME\"")
    );
}