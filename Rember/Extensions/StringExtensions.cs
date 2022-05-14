using CliFx.Exceptions;

namespace Rember.Extensions;

public static class StringExtensions
{
    /// <summary>
    ///     Throws <see cref="CommandException" /> if the string is empty.
    /// </summary>
    /// <param name="str">The string</param>
    /// <returns>The string</returns>
    /// <exception cref="CommandException">If the string is empty</exception>
    public static string ThrowCmdExceptionIfEmpty(this string str)
    {
        return str.ThrowCmdExceptionIfEmpty("Something went wrong");
    }

    /// <summary>
    ///     Throws <see cref="CommandException" /> if the string is empty.
    /// </summary>
    /// <param name="str">The string</param>
    /// <param name="msg">The exception message</param>
    /// <returns>The string</returns>
    /// <exception cref="CommandException">If the string is empty</exception>
    public static string ThrowCmdExceptionIfEmpty(this string str, string msg)
    {
        if (str.Length == 0) throw new CommandException(msg);

        return str;
    }

    /// <summary>
    ///     Removes all spaces from the given string
    /// </summary>
    /// <param name="str">The string</param>
    /// <returns>The same string without any spaces</returns>
    public static string RemoveSpaces(this string str)
    {
        return str.Replace(" ", "");
    }
}