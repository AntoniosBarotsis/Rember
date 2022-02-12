namespace Rember.Tasks;

/// <summary>
///     Implementation of the ITask interface.
/// </summary>
public class ConcreteTask : ITask
{
    // This is used for yml stuff, do not remove.
    public ConcreteTask()
    {
        Name = "";
        Command = "";
    }

    public ConcreteTask(string name, string command)
    {
        Name = name;
        Command = command;
    }

    public ConcreteTask(string name, string command, bool isEnabled, bool outputEnabled)
    {
        Name = name;
        Command = command;
        IsEnabled = isEnabled;
        OutputEnabled = outputEnabled;
    }

    public string Name { get; set; }
    public string Command { get; set; }
    public bool IsEnabled { get; set; } = true;
    public bool OutputEnabled { get; set; }

    public string GetName()
    {
        return Name;
    }

    public string GetCommand()
    {
        return Command;
    }
}