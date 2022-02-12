using System.Text.RegularExpressions;
using Rember.Tasks;

namespace Rember;

/// <summary>
///     Generic metadata stored in the hook files. As of now, that only includes the build tool.
/// </summary>
public class Metadata
{
    private Metadata(string[] data)
    {
        Data = data;
    }

    private string[] Data { get; }
    private static Regex MetadataRegex { get; } = new("#[a-z]+:[A-Za-z]+");

    /// <summary>
    ///     Retrieves the build tool that is specified in the hook metadata or null if it was not found.
    /// </summary>
    /// <returns>The BuildTool or null</returns>
    public BuildTool? GetBuildTool()
    {
        if (Data?[MetadataContents.BuildTool] is null) return null;

        return BuildTool.SupportedBuildTools
            .First(bt => bt.Name == Data[MetadataContents.BuildTool]);
    }

    /// <summary>
    ///     Parses a hook file and retrieves its metadata.
    /// </summary>
    /// <param name="text">The contents of the hook file</param>
    /// <returns>The Metadata</returns>
    public static Metadata Parse(string text)
    {
        var data = text
            .Split("\n")
            .Where(line => MetadataRegex.IsMatch(line))
            .Select(line => line.Split(":")[1].Trim())
            .ToArray();

        return new Metadata(data);
    }

    /// <summary>
    ///     Specifies the indices of different metadata elements.
    /// </summary>
    private static class MetadataContents
    {
        public static int BuildTool => 0;
    }
}