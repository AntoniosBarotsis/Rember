using Rember.FileStuff;
using Rember.Tasks;

namespace Rember.Actions;

public class ActionEditor
{
    public ActionEditor(Type type)
    {
        Type = type;

        // TODO This is ugly and repetitive, move to a separate clas FileAccessor or something
        var path = Directory.GetCurrentDirectory() + $"/.git/hooks/pre-{Type.ToString().ToLower()}";
        HookAccessor = new HookAccessor(path);

        Text = HookAccessor.Text;
        Metadata = HookAccessor.Metadata;

        BuildTool = Metadata.GetBuildTool() ?? 
                    throw new Exception("Something went wrong with the build tool retrieval");

    }

    private BuildTool BuildTool { get; }
    private Type Type { get; }
    private string Text { get; set; }
    private Metadata Metadata { get; }
    private HookAccessor HookAccessor { get; set; }

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
        HookAccessor.SaveChanges();
    }

    private string ToggleOutput(EditType editType)
    {
        var toggle = false;
        HookAccessor.Text = string.Join("\r\n", Text.Split("\r\n")
            .Select(line =>
            {
                // Next line has the command
                if (line.Contains("echo \"Running"))
                {
                    toggle = true;
                    return line;
                }

                if (!toggle) return line;
                toggle = false;
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