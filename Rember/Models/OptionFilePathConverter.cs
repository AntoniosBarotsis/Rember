using CliFx.Extensibility;
using Optional;

namespace Rember.Models;

public class OptionFilePathConverter : BindingConverter<Option<string>>
{
    public override Option<string> Convert(string? rawValue)
    {
        return rawValue.SomeNotNull()!;
    }
}