﻿using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Rember.Models;

/// <summary>
///     Represents the overall state of a hook file. Used for deserializing from yml.
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public record YmlStuff
{
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public string HookDirectory { get; set; } = "/.git/hooks";

    // ReSharper disable once CollectionNeverUpdated.Global
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public List<ConcreteTask> Tasks { get; set; } = new();

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