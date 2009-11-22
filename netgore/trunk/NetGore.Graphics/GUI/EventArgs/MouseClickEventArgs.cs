using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Event data for mouse button click events.
    /// </summary>
    public class MouseClickEventArgs : MouseEventArgs
    {
        /// <summary>
        /// Gets the <see cref="MouseButtons"/> used.
        /// </summary>
        public readonly MouseButtons Button;

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseClickEventArgs"/> class.
        /// </summary>
        /// <param name="button"><see cref="MouseButtons"/> used.</param>
        /// <param name="location">Location of the click relative to the sender.</param>
        public MouseClickEventArgs(MouseButtons button, Vector2 location) : base(location)
        {
            Button = button;
        }
    }
}