using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Rember.Models;

/// <summary>
///     Represents the overall state of a hook file. Used for serializing and deserializing to and from yml.
/// </summary>
public record YmlStuff
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

    /// <summary>
    ///     Turns the current instance to a yml string.
    /// </summary>
    /// <returns>The yml string</returns>
    public string Serialize()
    {
        return new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build()
            .Serialize(this);
    }

    /// <summary>
    ///     Turns the yml string to a class instance.
    /// </summary>
    /// <param name="yaml"></param>
    /// <returns>The <see cref="YmlStuff" /> instance</returns>
    public static YmlStuff Deserialize(string yaml)
    {
        return new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance) // see height_in_inches in sample yml 
            .Build()
            .Deserialize<YmlStuff>(yaml);
    }
}