namespace Rember.FileStuff;

public static class FileUtils
{
    private const string CommitHook = "pre-commit";
    private const string PushHook = "pre-push";
    private const string CommitSave = "pre-commit.txt";
    private const string PushSave = "pre-push.txt";
    private const string YmlFile = "./rember.yml";
    private static readonly string Path = Directory.GetCurrentDirectory() + "/.git/hooks/";
    public static readonly string PreCommitHookPath = Path + CommitHook;
    public static readonly string PrePushHookPath = Path + PushHook;
    public static readonly string PreCommitSavePath = Path + CommitSave;
    public static readonly string PrePushSavePath = Path + PushSave;
    public static readonly string YmlPath = Path + YmlFile;

    public static string ReadCommitHook()
    {
        return ReadFile(PreCommitHookPath);
    }

    public static string ReadPushHook()
    {
        return ReadFile(PrePushHookPath);
    }

    public static string ReadCommitSave()
    {
        return ReadFile(PreCommitSavePath);
    }

    public static string ReadPushSave()
    {
        return ReadFile(PrePushSavePath);
    }

    public static string ReadYml()
    {
        return ReadFile(YmlFile);
    }

    public static string ReadYml(string file)
    {
        return ReadFile(file);
    }

    private static string ReadFile(string file)
    {
        if (!File.Exists(file)) return $"File {file} is missing.";

        using var sr = new StreamReader(File.OpenRead(file));
        var text = sr.ReadToEnd();
        sr.Close();

        return text;
    }
}