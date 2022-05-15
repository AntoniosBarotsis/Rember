namespace Rember.Util;

/// <summary>
///     File writing abstraction.
/// </summary>
public class HookFileFacade
{
    private static HookFileFacade? _instance;

    public static string HookDirectory { get; set; } = "/.git/hooks";
    private readonly Lazy<string> _path 
        = new(() => {
                if (!HookDirectory.StartsWith("/")) HookDirectory = "/" + HookDirectory;

                return Directory.GetCurrentDirectory() +
                       Path.Join(
                           HookDirectory,
                           $"pre-{Type.ToString().ToLower()}");
            }
        );

    private HookFileFacade()
    {
        Text = "";
    }

    public string Text { get; private set; }
    private static Type Type => Type.Push;

    // Singleton
    public static HookFileFacade Instance
    {
        get { return _instance ??= new HookFileFacade(); }
    }

    /// <summary>
    ///     Stages a new <see cref="Text" /> value.
    /// </summary>
    /// <param name="text">The new Text</param>
    public void WriteToFile(string text)
    {
        Text = text;
    }

    /// <summary>
    ///     Dumps current value of <see cref="Text" /> to the file.
    /// </summary>
    public void SaveChanges()
    {
        if (!Directory.Exists(HookDirectory))
        {
            Directory.CreateDirectory(Path.Join(Directory.GetCurrentDirectory(), HookDirectory));
        }
        
        using var sw = new StreamWriter(File.Create(_path.Value));
        sw.Write(Text);
        sw.Close();
    }
}

public enum Type
{
    Commit,
    Push
}