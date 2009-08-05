using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Delegate for handling events using the MouseEventArgs
    /// </summary>
    /// <param name="sender">Event sender</param>
    /// <param name="e">Event args</param>
    public delegate void MouseEventHandler(object sender, MouseEventArgs e);

    /// <summary>
    /// Event data for mouse related events
    /// </summary>
    public class MouseEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the location of the click relative to the sender
        /// </summary>
        public readonly Vector2 Location;

        /// <summary>
        /// Gets the X-axis location of the click relative to the sender
        /// </summary>
        public float X
        {
            get { return Location.X; }
        }

        /// <summary>
        /// Gets the Y-axis location of the click relative to the sender
        /// </summary>
        public float Y
        {
            get { return Location.Y; }
        }

        /// <summary>
        /// MouseEventArgs constructor
        /// </summary>
        /// <param name="location">Location of the click relative to the sender</param>
        public MouseEventArgs(Vector2 location)
        {
            Location = location;
        }
    }
}