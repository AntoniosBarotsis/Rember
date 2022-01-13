using System.Globalization;
using System.Text.RegularExpressions;
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
    private string Text { get; }
    private Metadata Metadata { get; }
    private HookAccessor HookAccessor { get; }

    public string StageEdit(EditType editType, string taskName = "")
    {
        var res = editType switch
        {
            EditType.OutputEnable or EditType.OutputDisable => ToggleOutput(editType),
            EditType.TaskEnable => TaskEnable(taskName),
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

    private string TaskEnable(string taskName)
    {
        var textInfo = new CultureInfo("en-US", false).TextInfo;

        // Searches for the variable name to determine whether the task exists.
        if (!Text.Contains($"{taskName}Input", StringComparison.CurrentCultureIgnoreCase))
            return $"Task \"{taskName}\" not found.";

        // Extract index
        var tmp = new Regex($"echo \"Do you want to run {taskName}?", RegexOptions.IgnoreCase);
        var startIndex = tmp.Match(Text).Index;

        // Checks to see if this has already been commented out
        if (Text.Substring(startIndex - 2, 2) == "# ") return "Task already disabled";

        // Gets the rest of the text
        var rest = Text[startIndex..];
        var endIndex = rest.IndexOf("\n\r", StringComparison.Ordinal);
        var usefulPart = Text.Substring(startIndex, endIndex);

        // Comments out the 2 lines and sets the variable that was previously read to "n"
        var usefulPartUpdated = string.Join("\n", usefulPart
            .Split("\n")
            .ToList()
            .Select(el => $"# {el}")
            .Append($"{textInfo.ToTitleCase(taskName)}Input=\"n\""));

        HookAccessor.Text = Text.Replace(usefulPart, usefulPartUpdated);

        return $"Task \"{taskName}\" removed!";
    }
}

public enum EditType
{
    OutputEnable,
    OutputDisable,
    TaskDisable,
    TaskEnable
}