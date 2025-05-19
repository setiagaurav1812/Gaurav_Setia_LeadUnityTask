/// <summary>
/// Interface for implementing undoable and redoable commands.
/// </summary>
public interface ICommand
{
    /// <summary>
    /// Execute the command.
    /// </summary>
    void Execute();

    /// <summary>
    /// Undo the command.
    /// </summary>
    void Undo();

}