using System;
using System.Linq;

namespace NetGore.Editor
{
    public interface IUndoEvent : IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether or not Undo() has been called on this IUndoEvent.
        /// </summary>
        bool IsUndone { get; }

        /// <summary>
        /// How to redo this event once it has been undone.
        /// </summary>
        void Redo();

        /// <summary>
        /// How to undo this event.
        /// </summary>
        void Undo();
    }
}