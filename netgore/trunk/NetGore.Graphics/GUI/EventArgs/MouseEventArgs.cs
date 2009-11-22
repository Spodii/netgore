using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Event data for mouse related events.
    /// </summary>
    public class MouseEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the location of the click relative to the sender.
        /// </summary>
        public readonly Vector2 Location;

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseEventArgs"/> class.
        /// </summary>
        /// <param name="location">Location of the click relative to the sender.</param>
        public MouseEventArgs(Vector2 location)
        {
            Location = location;
        }

        /// <summary>
        /// Gets the X-axis location of the click relative to the sender.
        /// </summary>
        public float X
        {
            get { return Location.X; }
        }

        /// <summary>
        /// Gets the Y-axis location of the click relative to the sender.
        /// </summary>
        public float Y
        {
            get { return Location.Y; }
        }
    }
}