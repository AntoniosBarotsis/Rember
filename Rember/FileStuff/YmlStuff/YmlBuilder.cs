using Rember.Tasks;

namespace Rember.FileStuff.YmlStuff;

/// <summary>
///     Builds the skeleton of a <see cref="YmlStuff" /> instance.
/// </summary>
public class YmlBuilder
{
    public YmlBuilder(BuildTool buildTool)
    {
        BuildTool = buildTool;
        Tasks = new List<ConcreteTask>();
    }

    private BuildTool BuildTool { get; }
    private List<ConcreteTask> Tasks { get; }

    public YmlBuilder AddBuildTask()
    {
        Tasks.Add(BuildTool.Build);
        return this;
    }

    public YmlBuilder AddTestTask()
    {
        Tasks.Add(BuildTool.Test);
        return this;
    }

    public YmlBuilder AddTasks(IEnumerable<ConcreteTask> tasks)
    {
        Tasks.AddRange(tasks);
        return this;
    }

    public YmlStuff Build()
    {
        return new YmlStuff(BuildTool.Name, Tasks);
    }
}