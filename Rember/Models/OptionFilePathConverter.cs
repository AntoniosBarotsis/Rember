using CliFx.Extensibility;
using Optional;
using Rember.Commands;

namespace Rember.Models;

/// <summary>
///     Converts the command line arg from a string to an option of type string.
///     Used in <see cref="Init.ConfigPath" />.
/// </summary>
public class OptionFilePathConverter : BindingConverter<Option<string>>
{
    public override Option<string> Convert(string? rawValue)
    {
        return rawValue.SomeNotNull()!;
    }
}