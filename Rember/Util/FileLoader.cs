using Rember.Models;

namespace Rember.Util;

/// <summary>
///     Traverses through the project files.
/// </summary>
public class FileLoader
{
    private static FileLoader? _instance;

    private FileLoader()
    {
    }

    // Singleton
    public static FileLoader Instance
    {
        get { return _instance ??= new FileLoader(); }
    }

    private List<string>? Files { get; set; }

    /// <summary>
    ///     Recursively finds all files from the given path. The folder names found in
    ///     Language.Ignored are ignored.
    /// </summary>
    /// <param name="path">The path to search from.</param>
    /// <returns>A list of files.</returns>
    public Lazy<List<string>> DirectorySearch(string path)
    {
        Files ??= new List<string>(DirectorySearch(path, new List<string>()));
        return new Lazy<List<string>>(Files);
    }

    private static List<string> DirectorySearch(string path, List<string> files)
    {
        try
        {
            // Go over all files in current directory
            files.AddRange(Directory.GetFiles(path).Select(Path.GetFileName)!);

            // Remove ignored directories to save time
            foreach (var folder in Directory.GetDirectories(path))
            {
                var tmp = Path.GetFileName(folder).Replace(".", "");
                if (Language.Ignored.Contains(tmp)) continue;

                DirectorySearch(folder, files);
            }

            return files;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new List<string>();
        }
    }
}