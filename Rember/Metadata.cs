using System.Text.RegularExpressions;
using Rember.Tasks;

namespace Rember;

public class Metadata
{
    private Metadata(string[] data)
    {
        Data = data;
    }

    private string[] Data { get; }
    private static Regex MetadataRegex { get; } = new("#[a-z]+:[A-Za-z]+");

    public BuildTool? GetBuildTool()
    {
        if (Data?[MetadataContents.BuildTool] is null) return null;

        return BuildTool.SupportedBuildTools
            .First(bt => bt.Name == Data[MetadataContents.BuildTool]);
    }

    public static Metadata Parse(string text)
    {
        var data = text
            .Split("\n")
            .Where(line => MetadataRegex.IsMatch(line))
            .Select(line => line.Split(":")[1].Trim())
            .ToArray();

        return new Metadata(data);
    }

    private static class MetadataContents
    {
        public static int BuildTool => 0;
    }
}