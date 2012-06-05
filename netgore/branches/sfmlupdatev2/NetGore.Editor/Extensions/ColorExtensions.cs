using System.Linq;
using SFML.Graphics;

namespace NetGore.Editor
{
    public static class ColorExtensions
    {
        /// <summary>
        /// Creates a <see cref="System.Drawing.Color"/> from a <see cref="SFML.Graphics.Color"/>.
        /// </summary>
        /// <param name="color">The <see cref="SFML.Graphics.Color"/>.</param>
        /// <returns>The <see cref="System.Drawing.Color"/>.</returns>
        public static Color ToColor(this System.Drawing.Color color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }

        /// <summary>
        /// Creates a <see cref="System.Drawing.Color"/> from a <see cref="SFML.Graphics.Color"/>.
        /// </summary>
        /// <param name="color">The <see cref="SFML.Graphics.Color"/>.</param>
        /// <returns>The <see cref="System.Drawing.Color"/>.</returns>
        public static System.Drawing.Color ToSystemColor(this Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}