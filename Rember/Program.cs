using System.Reflection;
using CliFx;

var version = 
    Assembly.GetEntryAssembly()!
    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
    ?.InformationalVersion;

await new CliApplicationBuilder()
    .SetExecutableName("rember")
    .SetVersion($"v{version!}")
    .SetDescription("https://github.com/AntoniosBarotsis/Rember")
    .AddCommandsFromThisAssembly()
    .Build()
    .RunAsync();