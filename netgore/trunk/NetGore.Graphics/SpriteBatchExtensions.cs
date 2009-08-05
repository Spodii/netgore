using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Extensions for the SpriteBatch class.
    /// </summary>
    public static class SpriteBatchExtensions
    {
        /// <summary>
        /// Prepares the graphics device for drawing sprites with specified blending, sorting, and render state
        /// options, and a global transform matrix. Also makes the sprites drawn not use filtering.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw to.</param>
        public static void BeginUnfiltered(this SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            SetUnfiltered(spriteBatch);
        }

        /// <summary>
        /// Prepares the graphics device for drawing sprites with specified blending, sorting, and render state
        /// options, and a global transform matrix. Also makes the sprites drawn not use filtering.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw to.</param>
        /// <param name="blendMode">Blending options to use when rendering.</param>
        public static void BeginUnfiltered(this SpriteBatch spriteBatch, SpriteBlendMode blendMode)
        {
            spriteBatch.Begin(blendMode);
            SetUnfiltered(spriteBatch);
        }

        /// <summary>
        /// Prepares the graphics device for drawing sprites with specified blending, sorting, and render state
        /// options, and a global transform matrix. Also makes the sprites drawn not use filtering.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw to.</param>
        /// <param name="blendMode">Blending options to use when rendering.</param>
        /// <param name="sortMode">Sorting options to use when rendering.</param>
        /// <param name="stateMode">Rendering state options.</param>
        public static void BeginUnfiltered(this SpriteBatch spriteBatch, SpriteBlendMode blendMode, SpriteSortMode sortMode,
                                           SaveStateMode stateMode)
        {
            spriteBatch.Begin(blendMode, sortMode, stateMode);
            SetUnfiltered(spriteBatch);
        }

        /// <summary>
        /// Prepares the graphics device for drawing sprites with specified blending, sorting, and render state
        /// options, and a global transform matrix. Also makes the sprites drawn not use filtering.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw to.</param>
        /// <param name="blendMode">Blending options to use when rendering.</param>
        /// <param name="sortMode">Sorting options to use when rendering.</param>
        /// <param name="stateMode">Rendering state options.</param>
        /// <param name="transformMatrix">A matrix to apply to position, rotation, scale, and 
        /// depth data passed to SpriteBatch.Draw.</param>
        public static void BeginUnfiltered(this SpriteBatch spriteBatch, SpriteBlendMode blendMode, SpriteSortMode sortMode,
                                           SaveStateMode stateMode, Matrix transformMatrix)
        {
            spriteBatch.Begin(blendMode, sortMode, stateMode, transformMatrix);
            SetUnfiltered(spriteBatch);
        }

        /// <summary>
        /// Sets a SpriteBatch to not use filtering.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to not use filtering on.</param>
        static void SetUnfiltered(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.SamplerStates[0].MagFilter = TextureFilter.None;
        }
    }
}