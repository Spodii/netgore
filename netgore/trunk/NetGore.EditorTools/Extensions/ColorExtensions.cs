using System.Drawing;
using System.Linq;

namespace NetGore.EditorTools
{
    public static class ColorExtensions
    {
        /// <summary>
        /// Creates a <see cref="System.Drawing.Color"/> from a <see cref="SFML.Graphics.Color"/>.
        /// </summary>
        /// <param name="color">The <see cref="SFML.Graphics.Color"/>.</param>
        /// <returns>The <see cref="System.Drawing.Color"/>.</returns>
        public static SFML.Graphics.Color ToColor(this Color color)
        {
            return new SFML.Graphics.Color(color.R, color.G, color.B, color.A);
        }

        /// <summary>
        /// Creates a <see cref="System.Drawing.Color"/> from a <see cref="SFML.Graphics.Color"/>.
        /// </summary>
        /// <param name="color">The <see cref="SFML.Graphics.Color"/>.</param>
        /// <returns>The <see cref="System.Drawing.Color"/>.</returns>
        public static Color ToSystemColor(this SFML.Graphics.Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}