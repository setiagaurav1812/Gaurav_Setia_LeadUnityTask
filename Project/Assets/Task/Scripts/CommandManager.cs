using System;
using System.Collections.Generic;

/// <summary>
/// Handles command execution and provides undo/redo functionality.
/// </summary>
public class CommandManager
{
    #region Private Fields

    private Stack<ICommand> undoStack = new();
    private Stack<ICommand> redoStack = new();

    #endregion

    #region Events

    /// <summary>
    /// Event invoked when the state of undo/redo availability changes.
    /// </summary>
    public event Action<bool, bool> OnStateChanged;

    #endregion

    #region Public Methods

    /// <summary>
    /// Checks if undo is possible.
    /// </summary>
    public bool CanUndo() => undoStack.Count > 0;

    /// <summary>
    /// Checks if redo is possible.
    /// </summary>
    public bool CanRedo() => redoStack.Count > 0;

    /// <summary>
    /// Executes a command and pushes it onto the undo stack.
    /// </summary>
    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        undoStack.Push(command);
        redoStack.Clear();
        NotifyStateChanged();
    }

    /// <summary>
    /// Undoes the last executed command.
    /// </summary>
    public void Undo()
    {
        if (undoStack.Count > 0)
        {
            var command = undoStack.Pop();
            command.Undo();
            redoStack.Push(command);
            NotifyStateChanged();
        }
    }

    /// <summary>
    /// Redoes the last undone command.
    /// </summary>
    public void Redo()
    {
        if (redoStack.Count > 0)
        {
            var command = redoStack.Pop();
            command.Execute();
            undoStack.Push(command);
            NotifyStateChanged();
        }
    }

    #endregion

    #region Private Methods

    private void NotifyStateChanged()
    {
        OnStateChanged?.Invoke(CanUndo(), CanRedo());
    }

    #endregion
}