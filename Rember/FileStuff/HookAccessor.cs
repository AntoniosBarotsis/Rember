namespace Rember.FileStuff;

/// <summary>
///     Interfaces with the hook files.
/// </summary>
public class HookAccessor
{
    /// <summary>
    ///     Should be used for edits as it reads the file to populate the Text.
    /// </summary>
    /// <param name="path">The path to the hook</param>
    public HookAccessor(string path)
    {
        Path = path;
        Text = "";

        if (!File.Exists(path)) File.Create(path);

        using var sr = new StreamReader(File.OpenRead(path));
        Text = sr.ReadToEnd();

        Metadata = Metadata.Parse(Text);
        sr.Close();
    }

    /// <summary>
    ///     Should be used for file generations.
    /// </summary>
    /// <param name="path">The path to the hook</param>
    /// <param name="text">The file contents</param>
    public HookAccessor(string path, string text)
    {
        Path = path;
        Text = text;
        Metadata = Metadata.Parse(Text);
    }

    public string Text { get; set; }
    private string Path { get; }
    public Metadata Metadata { get; }

    /// <summary>
    ///     Overwrites the commit hook to the given text.
    /// </summary>
    /// <param name="text"></param>
    public static void WriteToCommitHook(string text)
    {
        WriteToHook(FileUtils.PreCommitHookPath, text);
    }

    /// <summary>
    ///     Overwrites the push hook to the given text.
    /// </summary>
    /// <param name="text"></param>
    public static void WriteToPushHook(string text)
    {
        WriteToHook(FileUtils.PrePushHookPath, text);
    }

    private static void WriteToHook(string path, string text)
    {
        using var sw = new StreamWriter(File.Create(path));
        sw.Write(text);
        sw.Close();
    }

    /// <summary>
    ///     Dumps the current commit hook to a save file.
    /// </summary>
    public static void SaveCommitHook()
    {
        SaveHook(FileUtils.PreCommitSavePath, FileUtils.ReadCommitHook());
    }

    /// <summary>
    ///     Dumps the current push hook to a save file.
    /// </summary>
    public static void SavePushHook()
    {
        SaveHook(FileUtils.PrePushSavePath, FileUtils.ReadPushHook());
    }

    private static void SaveHook(string path, string text)
    {
        using var sw = new StreamWriter(File.Create(path));
        sw.Write(text);
        sw.Close();
    }

    /// <summary>
    ///     Dump the Text to the file.
    /// </summary>
    public void SaveChanges()
    {
        using var sw = new StreamWriter(File.Create(Path));
        sw.Write(Text);
        sw.Close();
    }
}