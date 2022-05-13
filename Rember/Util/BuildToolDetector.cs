using Optional;
using Rember.Models;

namespace Rember.Util;

public static class BuildToolDetector
{
    /// <summary>
    ///     Tries to detect the build tool using the BuildTool.AssociatedFiles.
    /// </summary>
    /// <param name="files">The list of files from the directory in question.</param>
    /// <param name="language">The project language.</param>
    /// <returns>The detected build tool or null.</returns>
    public static Option<BuildTool> DetectBuildTool(IEnumerable<string> files, Language language)
    {
        if (language.BuildTools.Length == 1) return Option.Some(language.BuildTools[0]);

        var bt = (
            from f in files
            from buildTool in language.BuildTools
            where buildTool.AssociatedFiles.Contains(f)
            select buildTool
        ).FirstOrDefault();


        return bt.SomeNotNull()!;
    }

    public static Option<BuildTool> GetBuildTool(Lazy<List<string>> files)
    {
        var lang = LanguageDetector.DetectLanguage(files.Value);

        return lang.Match(
            l => DetectBuildTool(files.Value, l),
            Option.None<BuildTool>
        );
    }
}