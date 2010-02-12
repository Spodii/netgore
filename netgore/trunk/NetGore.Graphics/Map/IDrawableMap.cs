using System;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics
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
        /// Gets or sets a filter to be used when determining what components on the map will be drawn.
        /// If null, all components will be drawn (same as returning true for each value).
        /// </summary>
        Func<IDrawable, bool> DrawFilter { get; }

        /// <summary>
        /// Gets or sets if the particle effects should be drawn.
        /// </summary>
        bool DrawParticles { get; set; }

        /// <summary>
        /// Draws the map.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> to draw with.</param>
        void Draw(SpriteBatch spriteBatch);
    }
}