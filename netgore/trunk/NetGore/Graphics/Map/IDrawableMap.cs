using System;
using System.Linq;
using NetGore.World;

namespace NetGore.Graphics
{
    /// <summary>
    /// Interface for an <see cref="IMap"/> that supports being drawn.
    /// </summary>
    public interface IDrawableMap : IMap, ICamera2DProvider
    {
        /// <summary>
        /// Notifies listeners immediately before any of the map's layers are drawn.
        /// </summary>
        event TypedEventHandler<IDrawableMap, DrawableMapDrawEventArgs> BeginDrawMap;

        /// <summary>
        /// Notifies listeners immediately before a layer has started drawing.
        /// </summary>
        event TypedEventHandler<IDrawableMap, DrawableMapDrawLayerEventArgs> BeginDrawMapLayer;

        /// <summary>
        /// Notifies listeners immediately after any of the map's layers are drawn.
        /// </summary>
        event TypedEventHandler<IDrawableMap, DrawableMapDrawEventArgs> EndDrawMap;

        /// <summary>
        /// Notifies listeners immediately after a layer has finished drawing.
        /// </summary>
        event TypedEventHandler<IDrawableMap, DrawableMapDrawLayerEventArgs> EndDrawMapLayer;

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
        /// Adds a <see cref="ITemporaryMapEffect"/> to the map.
        /// </summary>
        /// <param name="e">The <see cref="ITemporaryMapEffect"/> to add.</param>
        void AddTemporaryMapEffect(ITemporaryMapEffect e);

        /// <summary>
        /// Draws the map.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw with.</param>
        void Draw(ISpriteBatch spriteBatch);
    }
}