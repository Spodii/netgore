using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Interface for an object that support drawing itself.
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// Notifies listeners immediately after this <see cref="IDrawable"/> is drawn.
        /// This event will be raised even if <see cref="IDrawable.IsVisible"/> is false.
        /// </summary>
        event IDrawableDrawEventHandler AfterDraw;

        /// <summary>
        /// Notifies listeners immediately before this <see cref="IDrawable"/> is drawn.
        /// This event will be raised even if <see cref="IDrawable.IsVisible"/> is false.
        /// </summary>
        event IDrawableDrawEventHandler BeforeDraw;

        /// <summary>
        /// Notifies listeners when the <see cref="IDrawable.Color"/> property has changed.
        /// </summary>
        event IDrawableEventHandler ColorChanged;

        /// <summary>
        /// Notifies listeners when the <see cref="IDrawable.MapRenderLayer"/> property has changed.
        /// </summary>
        event MapRenderLayerChange RenderLayerChanged;

        /// <summary>
        /// Notifies listeners when the <see cref="IDrawable.IsVisible"/> property has changed.
        /// </summary>
        event IDrawableEventHandler VisibleChanged;

        /// <summary>
        /// Gets or sets the <see cref="Color"/> to use when drawing this <see cref="IDrawable"/>. By default, this
        /// value will be equal to white (ARGB: 255,255,255,255).
        /// </summary>
        Color Color { get; set; }

        /// <summary>
        /// Gets or sets if this <see cref="IDrawable"/> will be drawn. All <see cref="IDrawable"/>s are initially
        /// visible.
        /// </summary>
        bool IsVisible { get; set; }

        /// <summary>
        /// Gets the depth of the object for the <see cref="IDrawable.MapRenderLayer"/> the object is on. A higher
        /// layer depth results in the object being drawn on top of (in front of) objects with a lower value.
        /// </summary>
        int LayerDepth { get; }

        /// <summary>
        /// Gets the <see cref="MapRenderLayer"/> that this object is rendered on.
        /// </summary>
        MapRenderLayer MapRenderLayer { get; }

        /// <summary>
        /// Makes the object draw itself.
        /// </summary>
        /// <param name="sb"><see cref="SpriteBatch"/> the object can use to draw itself with.</param>
        void Draw(SpriteBatch sb);

        /// <summary>
        /// Checks if in the object is in view of the specified <paramref name="camera"/>.
        /// </summary>
        /// <param name="camera">The <see cref="ICamera2D"/> to check if this object is in view of.</param>
        /// <returns>True if the object is in view of the camera, else False.</returns>
        bool InView(ICamera2D camera);
    }
}