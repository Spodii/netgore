using System.Drawing;
using System.Linq;
using SFML.Graphics;

namespace NetGore.Editor
{
    /// <summary>
    /// Extension methods for the <see cref="Size"/> struct.
    /// </summary>
    public static class SizeExtensions
    {
        /// <summary>
        /// Gets a <see cref="Vector2"/> from the <see cref="Size"/>.
        /// </summary>
        /// <param name="size">The <see cref="Size"/>.</param>
        /// <returns>The <see cref="Vector2"/>.</returns>
        public static Vector2 ToVector2(this Size size)
        {
            return new Vector2(size.Width, size.Height);
        }
    }
}