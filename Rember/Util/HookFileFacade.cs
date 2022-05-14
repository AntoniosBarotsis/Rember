namespace Rember.Util;

/// <summary>
///     File writing abstraction.
/// </summary>
public class HookFileFacade
{
    private static HookFileFacade? _instance;

    // Did this so it is possible to change the type to a commit for when and if that ever happens
    private readonly Lazy<string> _path = new(() =>
        Directory.GetCurrentDirectory() + $"/.git/hooks/pre-{Type.ToString().ToLower()}");

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