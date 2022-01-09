namespace Rember.Tasks;

public interface ITask
{
    /// <summary>
    /// Gets the name of the tool being used (eg Gradle)
    /// </summary>
    /// <param name="events">Left null for custom tasks, only used for predefined build tool actions.</param>
    /// <returns>The tool name.</returns>
    string GetToolName(Events? events = null);
    
    /// <summary>
    /// Gets the command to run.
    /// </summary>
    /// <param name="events">Left null for custom tasks, only used for predefined build tool actions.</param>
    /// <returns>Command</returns>
    string GetCommand(Events? events = null);
    
    /// <summary>
    /// Gets the description of the command (eg Build)
    /// </summary>
    /// <param name="events">Left null for custom tasks, only used for predefined build tool actions.</param>
    /// <returns>The command description</returns>
    string GetCommandDescription(Events? events = null);
}