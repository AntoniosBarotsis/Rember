using Rember.Util;

namespace Rember.Extensions;

public static class HookUtilsExtensions
{
    /// <summary>
    ///     Call <see cref="HookUtils.ToggleAutomaticRun" />.
    /// </summary>
    /// <param name="body"></param>
    /// <param name="alwaysRun"></param>
    /// <param name="taskName"></param>
    /// <returns>The edited body</returns>
    public static string SetAlwaysRunTo(this string body, bool alwaysRun, string taskName)
    {
        return HookUtils.ToggleAutomaticRun(body, taskName, alwaysRun);
    }

    /// <summary>
    ///     Enables/Disables task output
    /// </summary>
    /// <param name="text">The hook text that corresponds to the task</param>
    /// <param name="outputEnable">Whether to enable output or not</param>
    /// <returns>The edited text</returns>
    public static string ToggleOutput(this string text, bool outputEnable)
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
}