using Rember.Util;

namespace Rember.Extensions;

public static class TupleExtensions
{
    /// <summary>
    ///     Call <see cref="HookUtils.ToggleAutomaticRun" /> at the <code>body</code>.
    /// </summary>
    /// <param name="t">The tuple</param>
    /// <param name="alwaysRun"></param>
    /// <param name="taskName"></param>
    /// <returns>Same tuple with edited body</returns>
    public static (string header, string body) SetAlwaysRunTo(this (string header, string body) t, bool alwaysRun,
        string taskName)
    {
        return (t.header, HookUtils.ToggleAutomaticRun(t.body, taskName, alwaysRun));
    }
}