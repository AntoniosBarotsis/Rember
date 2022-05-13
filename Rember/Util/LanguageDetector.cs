using Optional;
using Rember.Models;

namespace Rember.Util;

/// <summary>
///     Tries to detect the language used in the project.
/// </summary>
public static class LanguageDetector
{
    /// <summary>
    ///     Detects the language used in the project or null if not found/supported. The language is
    ///     determined by the first known language specific file (for example package.json) or the first
    ///     known language extension (say .java).
    /// </summary>
    /// <param name="files">The list of files from the directory in question.</param>
    /// <returns>The Language.</returns>
    public static Option<Language> DetectLanguage(IEnumerable<string> files)
    {
        return (
            from f in files
            where f.Split(".").Length >= 2
            select new { filename = f, extension = f.Split(".")[1] }
            into tuple
            from supportedLanguage in Language.SupportedLanguages
            where
                supportedLanguage.Extensions.Contains(tuple.extension) ||
                supportedLanguage.AssociatedFiles.Contains(tuple.filename)
            select supportedLanguage
        ).FirstOrDefault().SomeNotNull()!;
    }
}