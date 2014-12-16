using System.Linq;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// The possible directions a text cursor can be moved.
    /// </summary>
    public enum MoveCursorDirection : byte
    {
        Up,
        Down,
        Left,
        Right,
        Start,
        End
    }
}