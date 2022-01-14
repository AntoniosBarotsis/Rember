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

    public ActionEditor(Type type, string text)
    {
        Type = type;
        
        // TODO This is ugly and repetitive, move to a separate clas FileAccessor or something
        var path = Directory.GetCurrentDirectory() + $"/.git/hooks/pre-{Type.ToString().ToLower()}";
        HookAccessor = new HookAccessor(path);

        Text = text;
        HookAccessor.Text = Text;
        
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
            EditType.TaskDisable => TaskDisable(taskName),
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

    /// <summary>
    /// Enables the given task
    /// </summary>
    /// <param name="taskName">The task name</param>
    /// <returns>Text output</returns>
    private string TaskEnable(string taskName)
    {
        // Searches for the variable name to determine whether the task exists.
        if (!TaskExists(taskName))
            return $"Task \"{taskName}\" not found.";

        // Extract index
        var tmp = new Regex($"echo \"Do you want to run {taskName}?", RegexOptions.IgnoreCase);
        var startIndex = tmp.Match(Text).Index - 2;

        // Checks to see if this has already been commented out
        if (Text.Substring(startIndex, 2) != "# ") return "Task already enabled";

        HookAccessor.Text = ToggleTask(startIndex, taskName, true);

        return $"Task \"{taskName}\" enabled!";
    }

    /// <summary>
    /// Disables the given task
    /// </summary>
    /// <param name="taskName">The task name</param>
    /// <returns>Text output</returns>
    private string TaskDisable(string taskName)
    {
        // Searches for the variable name to determine whether the task exists.
        if (!TaskExists(taskName))
            return $"Task \"{taskName}\" not found.";
        
        // Extract index
        var tmp = new Regex($"echo \"Do you want to run {taskName}?", RegexOptions.IgnoreCase);
        var startIndex = tmp.Match(Text).Index;

        // Checks to see if this has already been commented out
        if (Text.Substring(startIndex - 2, 2) == "# ") return "Task already disabled";
        
        HookAccessor.Text = ToggleTask(startIndex, taskName, false);
        
        return $"Task \"{taskName}\" disabled!";
    }

    /// <summary>
    /// Toggles the given task
    /// </summary>
    /// <param name="startIndex">The Text index where the task begins</param>
    /// <param name="taskName">The task name</param>
    /// <param name="enable">Whether you want the task enabled or disabled</param>
    /// <returns>Updated Text</returns>
    private string ToggleTask(int startIndex, string taskName, bool enable)
    {
        var textInfo = new CultureInfo("en-US", false).TextInfo;
        
        // Gets the rest of the text
        var rest = Text[startIndex..];
        var endIndex = rest.IndexOf("\n\r", StringComparison.Ordinal);
        var usefulPart = Text.Substring(startIndex, endIndex);

        var usefulPartUpdated = "";
        // Comments out the 2 lines and sets the variable that was previously read to "n"
        if (enable)
        {
            usefulPartUpdated = string.Join("\n", usefulPart
                .Split("\n")
                .ToList()
                .Select(el => el.Replace("# ", ""))
                .SkipLast(1));
        }
        else
        {
            usefulPartUpdated = string.Join("\n", usefulPart
                .Split("\n")
                .ToList()
                .Select(el => $"# {el}")
                .Append($"{textInfo.ToTitleCase(taskName)}Input=\"n\""));
        }

        return Text.Replace(usefulPart, usefulPartUpdated);
    }
    
    /// <summary>
    /// Returns true if the task exists.
    /// </summary>
    /// <param name="taskName">The task name</param>
    /// <returns>True iff task exists</returns>
    private bool TaskExists(string taskName)
    {
        return Text.Contains($"{taskName}Input", StringComparison.CurrentCultureIgnoreCase);
    }
}

public enum EditType
{
    OutputEnable,
    OutputDisable,
    TaskDisable,
    TaskEnable
}