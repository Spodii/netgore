using System.Linq;
using System.Windows.Forms;
using SFML.Graphics;

namespace NetGore.Editor
{
    /// <summary>
    /// Extension methods for the <see cref="MouseEventArgs"/>.
    /// </summary>
    public static class MouseEventArgsExtensions
    {
        /// <summary>
        /// Gets the position of a <see cref="MouseEventArgs"/> as a <see cref="Vector2"/>.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/>.</param>
        /// <returns>The position.</returns>
        public static Vector2 Position(this MouseEventArgs e)
        {
            return new Vector2(e.X, e.Y);
        }
    }
}