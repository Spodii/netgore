using System;
using System.Linq;
using SFML.Graphics;

namespace NetGore.Graphics
{
    // TODO: ## http://www.sfml-dev.org/tutorials/1.5/graphics-shape.php

    /// <summary>
    /// Assists in drawing untextured triangles. More accurately, it draws textures using
    /// a triangle texture (System.Triangle).
    /// </summary>
    public static class XNATriangle
    {
        /// <summary>
        /// Grh for the System.Blank GrhData that is used to draw the triangle
        /// </summary>
        static Grh _triangleGrh = null;

        /// <summary>
        /// Draws a triangle.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="dest">Destination rectangle.</param>
        /// <param name="color">Color of the box.</param>
        public static void Draw(ISpriteBatch sb, Rectangle dest, Color color)
        {
            Draw(sb, dest, color, SpriteEffects.None);
        }

        /// <summary>
        /// Draws a triangle.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="dest">Destination rectangle.</param>
        /// <param name="color">Color of the box.</param>
        /// <param name="effects">SpriteEffects to use.</param>
        public static void Draw(ISpriteBatch sb, Rectangle dest, Color color, SpriteEffects effects)
        {
            Draw(sb, dest, color, effects, 0f, Vector2.Zero);
        }

        /// <summary>
        /// Draws a triangle.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="dest">Destination rectangle.</param>
        /// <param name="color">Color of the box.</param>
        /// <param name="effects">SpriteEffects to use.</param>
        /// <param name="rotation">Rotation in radians.</param>
        /// <param name="origin">Relative origin of the rotation.</param>
        public static void Draw(ISpriteBatch sb, Rectangle dest, Color color, SpriteEffects effects, float rotation,
                                Vector2 origin)
        {
            LoadGrh();
            _triangleGrh.Draw(sb, dest, color, effects, rotation, origin);
        }

        /// <summary>
        /// Loads the _blankGrh, if needed, for drawing the triangle.
        /// </summary>
        static void LoadGrh()
        {
            if (_triangleGrh == null)
            {
                GrhData gd = GrhInfo.GetData("System", "Triangle");
                if (gd == null)
                    throw new Exception("Failed loading GrhData 'System.Triangle'");
                _triangleGrh = new Grh(gd);
            }
        }
    }
}