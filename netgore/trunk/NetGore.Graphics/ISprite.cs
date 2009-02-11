using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Interface for a 2D game sprite with basic drawing abilities
    /// </summary>
    public interface ISprite
    {
        /// <summary>
        /// Gets the source rectangle of the sprite on the texture
        /// </summary>
        Rectangle Source { get; }

        /// <summary>
        /// Gets the texture containing the sprite
        /// </summary>
        Texture2D Texture { get; }

        /// <summary>
        /// Draws the sprite
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw to</param>
        /// <param name="position">Position to draw to</param>
        /// <param name="color">Color to draw with</param>
        void Draw(SpriteBatch spriteBatch, Vector2 position, Color color);

        /// <summary>
        /// Draws the sprite
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw to</param>
        /// <param name="dest">Rectangle to draw to</param>
        /// <param name="color">Color to draw with</param>
        void Draw(SpriteBatch spriteBatch, Rectangle dest, Color color);
    }
}