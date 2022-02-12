namespace Rember.Tasks;

/// <summary>
///     Generic task interface
/// </summary>
public interface ITask
{
    /// <summary>
    ///     The name of the task.
    /// </summary>
    /// <returns>The task name</returns>
    string GetName();

    /// <summary>
    ///     Gets the command to execute.
    /// </summary>
    /// <returns>The command</returns>
    string GetCommand();
}