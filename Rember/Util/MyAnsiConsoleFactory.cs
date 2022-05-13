using CliFx.Infrastructure;
using Spectre.Console;

namespace Rember.Util;

public static class MyAnsiConsoleFactory
{
    public static IAnsiConsole CreateAnsiConsole(IConsole console)
    {
        return AnsiConsole.Create(new AnsiConsoleSettings
        {
            Ansi = AnsiSupport.Detect,
            ColorSystem = ColorSystemSupport.Detect,
            Out = new AnsiConsoleOutput(console.Output)
        });
    }
}