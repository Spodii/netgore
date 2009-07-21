using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Assists in drawing untextured rectangles. More accurately, it draws textures using
    /// a purely white texture (System.Blank).
    /// </summary>
    public static class XNARectangle
    {
        /// <summary>
        /// Grh for the System.Blank GrhData that is used to draw the rectangle
        /// </summary>
        static Grh _blankGrh = null;

        /// <summary>
        /// Draws a rectangle
        /// </summary>
        /// <param name="sb">SpriteBatch to draw to</param>
        /// <param name="dest">Destination rectangle</param>
        /// <param name="color">Color of the box</param>
        /// <param name="borderColor">Color of the border to draw around the rectangle</param>
        public static void Draw(SpriteBatch sb, Rectangle dest, Color color, Color borderColor)
        {
            LoadGrh();
            _blankGrh.Draw(sb, dest, color);

            // Check for a valid border color alpha
            if (borderColor.A == 0)
                return;

            // Get the points for the 4 corners
            Vector2 tl = new Vector2(dest.Left, dest.Top);
            Vector2 br = new Vector2(dest.Right, dest.Bottom);
            Vector2 tr = new Vector2(br.X, tl.Y);
            Vector2 bl = new Vector2(tl.X, br.Y);

            // Draw the 4 lines
            XNALine.Draw(sb, tl, tr, borderColor);
            XNALine.Draw(sb, tl, bl, borderColor);
            XNALine.Draw(sb, br, bl, borderColor);
            XNALine.Draw(sb, br, tr, borderColor);
        }

        /// <summary>
        /// Draws a rectangle
        /// </summary>
        /// <param name="sb">SpriteBatch to draw to</param>
        /// <param name="dest">Destination rectangle</param>
        /// <param name="color">Color of the box</param>
        public static void Draw(SpriteBatch sb, Rectangle dest, Color color)
        {
            LoadGrh();
            _blankGrh.Draw(sb, dest, color);
        }

        /// <summary>
        /// Loads the _blankGrh, if needed, for drawing the rectangle
        /// </summary>
        static void LoadGrh()
        {
            if (_blankGrh == null)
            {
                GrhData gd = GrhInfo.GetData("System", "Blank");
                if (gd == null)
                    throw new Exception("Failed to load GrhData System.Blank.");
                _blankGrh = new Grh(gd);
            }
        }
    }
}