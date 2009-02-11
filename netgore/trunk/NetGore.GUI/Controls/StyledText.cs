using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Describes a string of text and the style it uses.
    /// </summary>
    public class StyledText
    {
        /// <summary>
        /// Color of the text.
        /// </summary>
        public readonly Color Color;

        /// <summary>
        /// String of text.
        /// </summary>
        public readonly string Text;

        public StyledText(string text, Color color)
        {
            Text = text;
            Color = color;
        }
    }
}