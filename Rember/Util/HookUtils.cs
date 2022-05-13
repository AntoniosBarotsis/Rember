using System.Globalization;
using System.Text.RegularExpressions;
using CliFx.Exceptions;
using Rember.Extensions;
using Rember.Models;

namespace Rember.Util;

public class HookUtils
{
    public HookUtils(Type type, BuildTool buildTool)
    {
        Type = type;
        BuildTool = buildTool;
    }

    private Type Type { get; }
    private BuildTool BuildTool { get; }

    public (string header, string body) GenerateBuildScript(bool commandEnabled, bool outputEnabled = true)
    {
        return Generate(BuildTool.Build.GetName(), BuildTool.Build.GetCommand(), commandEnabled, outputEnabled);
    }

    public (string header, string body) GenerateTestScript(bool commandEnabled, bool outputEnabled = true)
    {
        return Generate(BuildTool.Test.GetName(), BuildTool.Test.GetCommand(), commandEnabled, outputEnabled);
    }

    public (string header, string body) GenerateAnyScript(string name, string command, bool commandEnabled,
        bool outputEnabled = true)
    {
        return Generate(name, command, commandEnabled, outputEnabled);
    }

    private (string header, string body) Generate(string name, string command, bool commandEnabled, bool outputEnabled)
    {
        var inputName = name.RemoveSpaces() + "Input";

        const string shebang = "#!/bin/sh\n\nexec < /dev/tty\n";
        const string header = $"{shebang}";

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
        body = ToggleOutput(body, outputEnabled);
        return (header, body);
    }

    private static string ToggleOutput(string text, bool outputEnable)
    {
        var toggle = false;
        return string.Join("\r\n", text.Split("\r\n")
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
                if (outputEnable) return line.Replace(" &> /dev/null", "");

                // This makes sure the out dump isn't added multiple times.
                return line.Contains(" &> /dev/null") ? line : line + " &> /dev/null";
            }).ToList());
    }

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

    private static int GetTaskStartIndex(string text, string taskName)
    {
        var tmp = new Regex($"echo \"Do you want to run {taskName}?", RegexOptions.IgnoreCase);
        return tmp.Match(text).Index;
    }

    private static bool TaskExists(string text, string task)
    {
        return text.Contains(task, StringComparison.CurrentCultureIgnoreCase);
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

    public static string EnableYes(string text, string taskName, bool alwaysRun)
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