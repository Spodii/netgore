using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Graphics;

namespace DemoGame.Client
{
    /// <summary>
    /// Interface for an <see cref="IMap"/> that supports being drawn.
    /// </summary>
    public interface IDrawableMap : IMap, ICamera2DProvider
    {
        /// <summary>
        /// Notifies listeners immediately before a layer has started drawing.
        /// </summary>
        event MapDrawEventHandler BeginDrawLayer;

        /// <summary>
        /// Notifies listeners immediately after a layer has finished drawing.
        /// </summary>
        event MapDrawEventHandler EndDrawLayer;

        /// <summary>
        /// Draws the map.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> to draw with.</param>
        void Draw(SpriteBatch spriteBatch);
    }
}