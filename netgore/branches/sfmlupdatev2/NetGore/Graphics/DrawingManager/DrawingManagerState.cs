using System.Linq;

namespace NetGore.Graphics
{
    /// <summary>
    /// The possible states of a <see cref="IDrawingManager"/>.
    /// </summary>
    public enum DrawingManagerState : byte
    {
        /// <summary>
        /// Nothing is current being drawn.
        /// </summary>
        Idle,

        /// <summary>
        /// The world is being drawn.
        /// </summary>
        DrawingWorld,

        /// <summary>
        /// The GUI is being drawn.
        /// </summary>
        DrawingGUI,
    }
}