using System.Globalization;
using System.Text.RegularExpressions;
using CliFx.Exceptions;
using Rember.Extensions;
using Rember.Models;

namespace Rember.Util;

/// <summary>
///     Exposes methods that produce the desired hook files.
/// </summary>
public class HookUtils
{
    public HookUtils(BuildTool buildTool)
    {
        BuildTool = buildTool;
    }

    private BuildTool BuildTool { get; }

    /// <summary>
    ///     Generates the hook contents for the build tool's build task.
    /// </summary>
    /// <param name="commandEnabled"></param>
    /// <param name="outputEnabled"></param>
    /// <param name="alwaysRun"></param>
    /// <returns>Tuple of the header (shebang and potential metadata) and the body</returns>
    public (string header, string body) GenerateBuildScript(bool commandEnabled, bool outputEnabled = true,
        bool alwaysRun = false)
    {
        return Generate(BuildTool.Build.GetName(), BuildTool.Build.GetCommand(), commandEnabled, outputEnabled,
            alwaysRun);
    }

    /// <summary>
    ///     Generates the hook contents for the build tool's test task.
    /// </summary>
    /// <param name="commandEnabled"></param>
    /// <param name="outputEnabled"></param>
    /// <param name="alwaysRun"></param>
    /// <returns>Tuple of the header (shebang and potential metadata) and the body</returns>
    public (string header, string body) GenerateTestScript(bool commandEnabled, bool outputEnabled = true,
        bool alwaysRun = false)
    {
        return Generate(BuildTool.Test.GetName(), BuildTool.Test.GetCommand(), commandEnabled, outputEnabled,
            alwaysRun);
    }

    /// <summary>
    ///     Like <see cref="GenerateBuildScript" /> and <see cref="GenerateBuildScript" /> but for custom tasks.
    /// </summary>
    /// <param name="name">Task name</param>
    /// <param name="command">Task command</param>
    /// <param name="commandEnabled"></param>
    /// <param name="outputEnabled"></param>
    /// <param name="alwaysRun"></param>
    /// <returns>Tuple of the header (shebang and potential metadata) and the body</returns>
    public static (string header, string body) GenerateAnyScript(string name, string command, bool commandEnabled,
        bool outputEnabled = true, bool alwaysRun = false)
    {
        return Generate(name, command, commandEnabled, outputEnabled, alwaysRun);
    }

    /// <summary>
    ///     Generates the hook file contents for any command and options.
    /// </summary>
    /// <param name="name">The task name</param>
    /// <param name="command">The command</param>
    /// <param name="commandEnabled"></param>
    /// <param name="outputEnabled"></param>
    /// <param name="alwaysRun"></param>
    /// <returns>Tuple of the header (shebang and potential metadata) and the body</returns>
    private static (string header, string body) Generate(string name, string command, bool commandEnabled,
        bool outputEnabled, bool alwaysRun)
    {
        var inputName = name.RemoveSpaces() + "Input";

        // exec < /dev/tty makes it so terminal input is read
        var header = alwaysRun ? "#!/bin/sh" : "#!/bin/sh\n\nexec < /dev/tty\n";

        var body = @$"
echo """"
echo ""Do you want to run {name}? [Y/n]""
read {inputName}
# {inputName}=""Y""

if [ -z ${inputName} ] || [ ${inputName} = ""Y"" ] || [ ${inputName} = ""y"" ]
then
    echo ""==============""
    echo ""Running {name} ({command})""
    {command} &> /dev/null
    status=$?

    if [ $status -eq 1 ]
    then
        echo ""{name} failed, exiting...""
    exit $status
        fi

    echo ""{name} passed!""
    echo ""==============""
    echo """"
    echo """"  
fi
";
        body = TaskToggle(body, name, commandEnabled);
        body = body
            .ToggleOutput(outputEnabled)
            .SetAlwaysRunTo(alwaysRun, name);
        
        return (header, body);
    }

    /// <summary>
    ///     Enables/Disables a task
    /// </summary>
    /// <param name="text">The hook text that corresponds to the task</param>
    /// <param name="taskName">The task name</param>
    /// <param name="enable">Whether to enable it or not</param>
    /// <returns>The edited text</returns>
    /// <exception cref="CommandException">If the task was not found</exception>
    private static string TaskToggle(string text, string taskName, bool enable)
    {
        // Searches for the variable name to determine whether the task exists.
        if (!TaskExists(text, taskName))
            throw new CommandException($"Task \"{taskName}\" not found.");

        // Extract index
        var startIndex = GetTaskStartIndex(text, taskName);

        // Checks to see if this has already been commented out
        var isCommented = text.Substring(startIndex, 2) == "# ";

        if ((isCommented && !enable) || (!isCommented && enable))
            return text;

        return TaskToggleHelper(text, startIndex, taskName, enable);
    }

    /// <summary>
    ///     Gets start index of the <code>Do you want to run TASK_NAME</code> line.
    /// </summary>
    /// <param name="text">The text</param>
    /// <param name="taskName">The task name</param>
    /// <returns>The index of the beginning of the line (or -1 if not found)</returns>
    private static int GetTaskStartIndex(string text, string taskName)
    {
        var tmp = new Regex($"echo \"Do you want to run {taskName}?", RegexOptions.IgnoreCase);
        return tmp.Match(text).Index;
    }

    /// <summary>
    ///     Returns true if the task exists.
    /// </summary>
    /// <param name="text">The whole hook file</param>
    /// <param name="taskName">The task name</param>
    /// <returns></returns>
    private static bool TaskExists(string text, string taskName)
    {
        return text.Contains(taskName, StringComparison.CurrentCultureIgnoreCase);
    }

    /// <summary>
    ///     Does the actual comment toggling.
    /// </summary>
    /// <param name="text">The hook contents</param>
    /// <param name="startIndex">Where the task starts</param>
    /// <param name="taskName">Task name</param>
    /// <param name="enable">Whether to enable the task or not</param>
    /// <returns>Updated hook contents</returns>
    private static string TaskToggleHelper(string text, int startIndex, string taskName, bool enable)
    {
        var textInfo = new CultureInfo("en-US", false).TextInfo;

        // Gets the rest of the text
        var rest = text[startIndex..];
        var endIndex = rest.IndexOf("\n\r", StringComparison.Ordinal);
        var usefulPart = text.Substring(startIndex, endIndex);

        var usefulPartUpdated = "";
        // Comments out the 2 lines and sets the variable that was previously read to "n"
        if (enable)
            usefulPartUpdated = string.Join("\n", usefulPart
                .Split("\n")
                .AsEnumerable()
                .Select(el => el.Replace("# ", ""))
                .SkipLast(1));
        else
            usefulPartUpdated = string.Join("\n", usefulPart
                .Split("\n")
                .AsEnumerable()
                .Select(el => $"# {el}")
                .Append($"{textInfo.ToTitleCase(taskName)}Input=\"n\""));

        return text.Replace(usefulPart, usefulPartUpdated);
    }

    /// <summary>
    ///     Enables/disables automatic run of a task
    /// </summary>
    /// <param name="text">The task text</param>
    /// <param name="taskName">The task name</param>
    /// <param name="alwaysRun"></param>
    /// <returns>The edited text</returns>
    public static string ToggleAutomaticRun(string text, string taskName, bool alwaysRun)
    {
        if (!alwaysRun) return text;

        var index = GetTaskStartIndex(text, taskName);
        var rest = text[index..];
        var endIndex = rest.IndexOf("=\"Y\"", StringComparison.Ordinal) + "Input".Length;
        var usefulPart = text.Substring(index, endIndex);
        var usefulPartArr = usefulPart.Split("\n");
        var commented =
            string.Join("\n", usefulPartArr.Select(s => $"# {s}").Take(1))
            + "\n" + usefulPartArr[^1].Replace("# ", "");
        text = text.Replace(usefulPart, commented);

        return text;
    }
}