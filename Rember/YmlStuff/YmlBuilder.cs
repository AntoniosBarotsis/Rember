using Rember.Tasks;

namespace Rember.YmlStuff;

public class YmlBuilder
{
    private BuildTool BuildTool { get; set; }
    private List<ConcreteTask> Tasks { get; set; }

    public YmlBuilder(BuildTool buildTool)
    {
        BuildTool = buildTool;
        Tasks = new List<ConcreteTask>();
    }

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