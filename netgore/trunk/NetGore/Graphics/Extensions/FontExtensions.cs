using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Extension methods for the <see cref="Font"/> class.
    /// </summary>
    public static class FontExtensions
    {
        static readonly String2D _s = new String2D();
        
        /// <summary>
        /// Measures the size of a string.
        /// </summary>
        /// <param name="font">The <see cref="Font"/>.</param>
        /// <param name="str">The string to measure.</param>
        /// <returns>The size of the <paramref name="str"/>.</returns>
        public static Vector2 MeasureString(this Font font, string str)
        {
            _s.Font = font;
            _s.Size = font.CharacterSize;
            _s.Text = str;

            var r = _s.GetRect();

            return new Vector2(r.Width, r.Height);
        }

        /// <summary>
        /// Measures the size of a string.
        /// </summary>
        /// <param name="font">The <see cref="Font"/>.</param>
        /// <param name="str">The string to measure.</param>
        /// <returns>The size of the <paramref name="str"/>.</returns>
        public static Vector2 MeasureString(this Font font, StringBuilder str)
        {
            return MeasureString(font, str.ToString());
        }
    }
}
