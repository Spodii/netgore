using System.Linq;
using Microsoft.Xna.Framework;
using NetGore;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Delegate for handling events using the MouseClickEventArgs
    /// </summary>
    /// <param name="sender">Event sender</param>
    /// <param name="e">Event args</param>
    public delegate void MouseClickEventHandler(object sender, MouseClickEventArgs e);

    /// <summary>
    /// Event data for mouse button click events
    /// </summary>
    public class MouseClickEventArgs : MouseEventArgs
    {
        /// <summary>
        /// Gets the MouseButton used
        /// </summary>
        public readonly MouseButtons Button;

        /// <summary>
        /// MouseClickEventArgs constructor
        /// </summary>
        /// <param name="button">MouseButton used</param>
        /// <param name="location">Location of the click relative to the sender</param>
        public MouseClickEventArgs(MouseButtons button, Vector2 location) : base(location)
        {
            Button = button;
        }
    }
}