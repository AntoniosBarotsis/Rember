namespace Rember.Tasks;

public class CustomTask: ITask
{
    private string Name { get; }
    private string Command { get; }

    public CustomTask(string name, string command)
    {
        Name = name;
        Command = command;
    }

    public string GetName()
    {
        return Name;
    }

    public string GetCommand()
    {
        return Command;
    }
}