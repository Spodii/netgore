using System.Collections.Generic;

namespace NetGore.EditorTools
{
    public interface IUndoManager
    {
        /// <summary>
        /// Adds an event to the list, clears any events that have previously been undone.
        /// </summary>
        /// <param name="undoEvent">Event to be added to the list.</param>
        void Push(IUndoEvent undoEvent);

        /// <summary>
        /// Pops the IUndoEvent that the cursor is currently on, but leaves it available for Redoing.
        /// </summary>
        /// <returns>The IUndoEvent at the current index.</returns>
        IUndoEvent Pop();

        /// <summary>
        /// Gets all events currently stored, including those that have been undone.
        /// </summary>
        IEnumerable<IUndoEvent> Events { get; }

        /// <summary>
        /// Gets the index of the event that the cursor is on. 
        /// </summary>
        int CursorIndex { get; }

        /// <summary>
        /// Attempts to get the IUndoEvent at the specified index.
        /// </summary>
        /// <param name="index">Index of the desired IUndoEvent.</param>
        /// <returns>The IUndoEvent at the specified index if it exists, if not - null.</returns>
        IUndoEvent TryGetEvent(int index);

        /// <summary>
        /// Gets the IUndoEvent that the cursor is currently on.
        /// </summary>
        IUndoEvent CurrentEvent { get; }

        /// <summary>
        /// Attempts to undo the last event that was added to the list.
        /// </summary>
        /// <returns>True if successful, false if not.</returns>
        bool TryUndo(); // Try to pop the last event, and if it exists, call Undo on it.

        /// <summary>
        /// Attempts to redo the last undone event.
        /// </summary>
        /// <returns>True if successful, false if not.</returns>
        bool TryRedo(); // If there are any events you can call "Redo" on, call it. Otherwise, false.
    }
}