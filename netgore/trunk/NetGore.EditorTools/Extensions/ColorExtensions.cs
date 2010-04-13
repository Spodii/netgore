using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.EditorTools
{
    public static class ColorExtensions
    {
        /// <summary>
        /// Creates a <see cref="System.Drawing.Color"/> from a <see cref="SFML.Graphics.Color"/>.
        /// </summary>
        /// <param name="color">The <see cref="SFML.Graphics.Color"/>.</param>
        /// <returns>The <see cref="System.Drawing.Color"/>.</returns>
        public static System.Drawing.Color ToSystemColor(this SFML.Graphics.Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// Creates a <see cref="System.Drawing.Color"/> from a <see cref="SFML.Graphics.Color"/>.
        /// </summary>
        /// <param name="color">The <see cref="SFML.Graphics.Color"/>.</param>
        /// <returns>The <see cref="System.Drawing.Color"/>.</returns>
        public static SFML.Graphics.Color ToColor(this System.Drawing.Color color)
        {
            return new SFML.Graphics.Color(color.R, color.G, color.B, color.A);
        }
    }
}
