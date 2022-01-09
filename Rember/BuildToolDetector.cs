using Rember.Tasks;

namespace Rember;

public static class BuildToolDetector
{
    /// <summary>
    ///     Tries to detect the build tool using the BuildTool.AssociatedFiles.
    /// </summary>
    /// <param name="files">The list of files from the directory in question.</param>
    /// <param name="language">The project language.</param>
    /// <returns>The detected build tool or null.</returns>
    public static BuildTool? DetectBuildTool(IEnumerable<string> files, Language language)
    {
        if (language.BuildTools.Length == 1) return language.BuildTools[0];

        return (
            from f in files
            from buildTool in language.BuildTools
            where buildTool.AssociatedFiles.Contains(f)
            select buildTool
        ).FirstOrDefault();
    }
}