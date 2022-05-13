using Rember.Util;

namespace Rember.Extensions;

public static class TupleExtensions
{
    public static (string header, string body) SetAlwaysRunTo(this (string header, string body) t, bool alwaysRun,
        string taskName)
    {
        return (t.header, HookUtils.EnableYes(t.body, taskName, alwaysRun));
    }
}