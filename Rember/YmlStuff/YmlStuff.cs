using Rember.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Rember.YmlStuff;

public class YmlStuff
{
    public YmlStuff()
    {
        BuildToolName = "";
        Tasks = new List<ConcreteTask>();
    }

    public YmlStuff(string buildToolName)
    {
        BuildToolName = buildToolName;
        Tasks = new List<ConcreteTask>();
    }

    public YmlStuff(string buildToolName, List<ConcreteTask> tasks)
    {
        BuildToolName = buildToolName;
        Tasks = tasks;
    }

    public string BuildToolName { get; set; }
    public List<ConcreteTask> Tasks { get; set; }

    public string Serialize()
    {
        return new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build()
            .Serialize(this);
    }

    public static YmlStuff Deserialize(string yaml)
    {
        return new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance) // see height_in_inches in sample yml 
            .Build()
            .Deserialize<YmlStuff>(yaml);
    }
}