using System.Text.RegularExpressions;

namespace Rember;

public class ActionEditor
{
    public ActionEditor(Type type)
    {
        Type = type;

        // TODO This is ugly and repetitive, move to a separate clas FileAccessor or something
        var path = Directory.GetCurrentDirectory() + $"/.git/hooks/pre-{Type.ToString().ToLower()}";
        using var sr = new StreamReader(File.OpenRead(path));
        Text = sr.ReadToEnd();

        Metadata = Text
            .Split("\n")
            .Where(line => MetadataRegex.IsMatch(line))
            .Select(line => line.Split(":")[1].Trim())
            .ToArray();

        BuildTool = BuildTool.SupportedBuildTools
            .First(bt => bt.Name == Metadata[0]);
    }

    private BuildTool BuildTool { get; }
    private Type Type { get; }
    private string Text { get; set; }
    private string[] Metadata { get; }
    private static Regex MetadataRegex { get; } = new("#[a-z]+:[A-Za-z]+");

    public string StageEdit(EditType editType)
    {
        var res = editType switch
        {
            EditType.OutputEnable or EditType.OutputDisable => ToggleOutput(editType),
            _ => ""
        };

        return res;
    }

    public void ApplyEdits()
    {
        var path = Directory.GetCurrentDirectory() + $"/.git/hooks/pre-{Type.ToString().ToLower()}";
        using var sw = new StreamWriter(File.Create(path));
        sw.Write(Text);
    }

    private string ToggleOutput(EditType editType)
    {
        Text = string.Join("\r\n", Text.Split("\r\n")
            .Select(line =>
            {
                if (!(line.Contains(BuildTool.Build) || line.Contains(BuildTool.Test))) return line;

                if (editType == EditType.OutputEnable) return line.Replace(" &> /dev/null", "");

                // This makes sure the out dump isn't added multiple times.
                return line.Contains(" &> /dev/null") ? line : line + " &> /dev/null";
            }).ToList());

        return editType == EditType.OutputEnable ? "Output enabled." : "Output disabled";
    }
}

public enum EditType
{
    OutputEnable,
    OutputDisable
}