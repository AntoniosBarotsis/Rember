namespace Rember.FileStuff;

/// <summary>
/// Interfaces with the hook files.
/// </summary>
public class HookAccessor
{
    public string Text { get; set; }
    private string Path { get; }
    public Metadata Metadata { get; }

    /// <summary>
    /// Should be used for edits as it reads the file to populate the Text.
    /// </summary>
    /// <param name="path">The path to the hook</param>
    public HookAccessor(string path)
    {
        Path = path;
        Text = "";

        if (!File.Exists(path))
        {
            File.Create(path);
        }
        
        using var sr = new StreamReader(File.OpenRead(path));
        Text = sr.ReadToEnd();

        Metadata = Metadata.Parse(Text);
        sr.Close();
    }

    /// <summary>
    /// Should be used for file generations.
    /// </summary>
    /// <param name="path">The path to the hook</param>
    /// <param name="text">The file contents</param>
    public HookAccessor(string path, string text)
    {
        Path = path;
        Text = text;
        Metadata = Metadata.Parse(Text);
    }

    /// <summary>
    /// Dump the Text to the file.
    /// </summary>
    public void SaveChanges()
    {
        using var sw = new StreamWriter(File.Create(Path));
        sw.Write(Text);
        sw.Close();
    }
}