using System.Linq;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Interface for a class that contains text that is able to be edited through input devices.
    /// </summary>
    public interface IEditableText
    {
        /// <summary>
        /// Breaks the line at the current position of the text cursor.
        /// </summary>
        void BreakLine();

        /// <summary>
        /// Deletes the character from the <see cref="Control"/>'s text immediately before the current position
        /// of the text cursor. When applicable, if the cursor is at the start of the line, the cursor be moved
        /// to the previous line and the remainder of the line will be appended to the end of the previous line.
        /// </summary>
        void DeleteCharLeft();

        /// <summary>
        /// Deletes the character from the <see cref="Control"/>'s text immediately after the current position
        /// of the text cursor. When applicable, if the cursor is at the end of the line, the cursor be moved
        /// to the next line and the remainder of the line will be appended to the start of the previous line.
        /// </summary>
        void DeleteCharRight();

        /// <summary>
        /// Inserts the specified character to the <see cref="Control"/>'s text at the current position
        /// of the text cursor.
        /// </summary>
        /// <param name="c">The character to insert.</param>
        void InsertChar(string c);

        /// <summary>
        /// Moves the cursor in the specified <paramref name="direction"/> by one character.
        /// </summary>
        /// <param name="direction">The direction to move the cursor.</param>
        void MoveCursor(MoveCursorDirection direction);
    }
}