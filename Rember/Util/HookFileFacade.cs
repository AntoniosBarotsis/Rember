using Rember.Models;

namespace Rember.Util;

public class HookFileFacade
{
    private static HookFileFacade? _instance;

    private readonly Lazy<string> _path = new(() =>
        Directory.GetCurrentDirectory() + $"/.git/hooks/pre-{Type.ToString().ToLower()}");

    private HookFileFacade()
    {
        Text = "";
    }

    public string Text { get; private set; }
    private static Type Type { get; } = Type.Push;

    // Singleton
    public static HookFileFacade Instance
    {
        get { return _instance ??= new HookFileFacade(); }
    }

    /// <summary>
    ///     Reads current value of the hook file into the <see cref="Text" />.
    /// </summary>
    public void ReadFromFile()
    {
        using var sr = new StreamReader(File.OpenRead(_path.Value));
        Text = sr.ReadToEnd();
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
    ///     Stages a new <see cref="Text" /> by appending to it.
    /// </summary>
    /// <param name="text">The Text to append</param>
    public void AppendToFile(string text)
    {
        Text += text;
    }

    /// <summary>
    ///     Dumps current value of <see cref="Text" /> to the file.
    /// </summary>
    public void SaveChanges()
    {
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