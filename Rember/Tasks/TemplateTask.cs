namespace Rember.Tasks;

public class TemplateTask: ITask
{
    private string Name { get; }
    private string Command { get; }

    public TemplateTask(string name, string command)
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