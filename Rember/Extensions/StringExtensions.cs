using CliFx.Exceptions;

namespace Rember.Extensions;

public static class StringExtensions
{
    public static string ThrowCmdExceptionIfEmpty(this string str)
    {
        return str.ThrowCmdExceptionIfEmpty("Something went wrong");
    }

    public static string ThrowCmdExceptionIfEmpty(this string str, string msg)
    {
        if (str.Length == 0) throw new CommandException(msg);

        return str;
    }

    public static string ThrowIfEmpty(this string str, Exception e)
    {
        if (str.Length == 0) throw e;

        return str;
    }

    public static string RemoveSpaces(this string str)
    {
        return str.Replace(" ", "");
    }
}