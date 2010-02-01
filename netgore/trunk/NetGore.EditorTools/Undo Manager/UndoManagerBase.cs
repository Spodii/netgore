using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.EditorTools
{
    public class UndoManagerBase : IUndoManager
    {
        /// <summary>
        /// Collection of IUndoEvents.
        /// </summary>
        readonly List<IUndoEvent> _events = new List<IUndoEvent>();

        /// <summary>
        /// Current index that the cursor is on.
        /// </summary>
        int _cursorIndex;

        #region IUndoManager Members

        /// <summary>
        /// Gets the IUndoEvent that the cursor is currently on.
        /// </summary>
        public IUndoEvent CurrentEvent
        {
            get { return TryGetEvent(_cursorIndex); }
        }

        /// <summary>
        /// Gets the index of the event that the cursor is on. 
        /// </summary>
        public int CursorIndex
        {
            get { return _cursorIndex; }
        }

        /// <summary>
        /// Gets all events currently stored.
        /// </summary>
        public IEnumerable<IUndoEvent> Events
        {
            get { return _events; }
        }

        /// <summary>
        /// Pops the IUndoEvent that the cursor is currently on, but leaves it available for Redoing.
        /// </summary>
        /// <returns>The IUndoEvent at the current index.</returns>
        public IUndoEvent Pop()
        {
            //Store the current cursor index because I can't figure out a better way to do this.
            int cu = _cursorIndex;

            //Move the cursor down by 1 if it isn't already at 0.
            _cursorIndex -= _cursorIndex != 0 ? 1 : 0;

            //Just a little exception mangling.
            try
            {
                return _events[cu];
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new InvalidOperationException("No events to pop.");
            }
        }

        /// <summary>
        /// Adds an IUndoEvent to the list, clears any events that have previously been undone.
        /// </summary>
        /// <param name="undoEvent">Event to be added to the list.</param>
        public void Push(IUndoEvent undoEvent)
        {
            //If there are any undone events (events after the cursor index in the list), remove them.
            if (_cursorIndex < _events.Count() - 1)
            {
                foreach (IUndoEvent eve in _events.GetRange(_cursorIndex + 1, _events.Count() - (_cursorIndex + 1)))
                {
                    eve.Dispose();
                }

                _events.RemoveRange(_cursorIndex + 1, _events.Count() - (_cursorIndex + 1));
            }

            //Add the IUndoEvent to the list.
            _events.Add(undoEvent);

            //Move the cursor to the end of the buffer.
            _cursorIndex = _events.Count() - 1;
        }

        /// <summary>
        /// Attempts to get the IUndoEvent at the specified index.
        /// </summary>
        /// <param name="index">Index of the desired IUndoEvent.</param>
        /// <returns>The IUndoEvent at the specified index if it exists, if not - null.</returns>
        public IUndoEvent TryGetEvent(int index)
        {
            try
            {
                return _events[index];
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        /// <summary>
        /// Attempts to redo the last undone event.
        /// </summary>
        /// <returns>True if successful, false if not.</returns>
        public bool TryRedo()
        {
            int indexModifier;

            //Checks next and current indexes, if neither are undoable events - return false.
            if (TryGetEvent(_cursorIndex + 1) != null)
                indexModifier = 1;
            else if (TryGetEvent(_cursorIndex).IsUndone)
                indexModifier = 0;
            else
                return false;

            //Redo either the event at the current index or the one after (determined by the above conditional)
            _events[_cursorIndex + indexModifier].Redo();
            _cursorIndex += indexModifier;
            return true;
        }

        /// <summary>
        /// Attempts to undo the last event that was added to the list.
        /// </summary>
        /// <returns>True if successful, false if not.</returns>
        public bool TryUndo()
        {
            try
            {
                //Pop the current event and then undo it.
                IUndoEvent eventToUndo = Pop();
                eventToUndo.Undo();

                //If successful, return true; otherwise, false!
                if (eventToUndo.IsUndone)
                    return true;
                return false;
            }
            catch (InvalidOperationException)
            {
                //The IUndoEvent doesn't exist.
                return false;
            }
        }

        #endregion
    }
}