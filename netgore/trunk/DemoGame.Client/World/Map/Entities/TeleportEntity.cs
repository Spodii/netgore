using System.ComponentModel;
using System.Linq;
using NetGore;
using NetGore.Graphics;
using SFML.Graphics;

namespace DemoGame.Client
{
    [MapFileEntity]
    public class TeleportEntity : TeleportEntityBase, IDrawable
    {
        Color _color = new Color(255, 255, 255, 100);
        bool _isVisible = true;

        /// <summary>
        /// Notifies the listeners when the IUsableEntity was used, and the DynamicEntity that used it. On the Client, this
        /// event will only be triggered if NotifyClientsOfUsage is true. The DynamicEntity argument
        /// that used this IUsableEntity may be null.
        /// </summary>
        public override event EntityEventHandler<DynamicEntity> OnUse;

        /// <summary>
        /// Client:
        ///     Handles any additional usage stuff. When this is called, it is to be assumed that the Server has recognized
        ///     the IUsableEntity as having been successfully used.
        /// Server:
        ///     Attempts to use this IUsableEntity on the <paramref name="charEntity"/>.
        /// </summary>
        /// <param name="charEntity">CharacterEntity that is trying to use this IUsableEntity. Can be null.</param>
        /// <returns>True if this IUsableEntity was successfully used, else false. On the Client, this is generally
        /// unused.</returns>
        public override bool Use(DynamicEntity charEntity)
        {
            if (OnUse != null)
                OnUse(this, charEntity);

            return true;
        }

        #region IDrawable Members

        /// <summary>
        /// Notifies listeners immediately after this <see cref="IDrawable"/> is drawn.
        /// This event will be raised even if <see cref="IDrawable.IsVisible"/> is false.
        /// </summary>
        public event IDrawableDrawEventHandler AfterDraw;

        /// <summary>
        /// Notifies listeners immediately before this <see cref="IDrawable"/> is drawn.
        /// This event will be raised even if <see cref="IDrawable.IsVisible"/> is false.
        /// </summary>
        public event IDrawableDrawEventHandler BeforeDraw;

        /// <summary>
        /// Notifies listeners when the <see cref="IDrawable.Color"/> property has changed.
        /// </summary>
        public event IDrawableEventHandler ColorChanged;

        /// <summary>
        /// Unused by the <see cref="TeleportEntity"/> since the layer never changes.
        /// </summary>
        event MapRenderLayerChange IDrawable.RenderLayerChanged
        {
            add { }
            remove { }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="IDrawable.IsVisible"/> property has changed.
        /// </summary>
        public event IDrawableEventHandler VisibleChanged;

        /// <summary>
        /// Gets or sets the <see cref="IDrawable.Color"/> to use when drawing this <see cref="IDrawable"/>. By default, this
        /// value will be equal to white (ARGB: 255,255,255,255).
        /// </summary>
        public Color Color
        {
            get { return _color; }
            set
            {
                if (_color == value)
                    return;

                _color = value;

                if (ColorChanged != null)
                    ColorChanged(this);
            }
        }

        /// <summary>
        /// Gets or sets if this <see cref="IDrawable"/> will be drawn. All <see cref="IDrawable"/>s are initially
        /// visible.
        /// </summary>
        [Browsable(false)]
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible == value)
                    return;

                _isVisible = value;

                if (VisibleChanged != null)
                    VisibleChanged(this);
            }
        }

        /// <summary>
        /// Gets the depth of the object for the <see cref="IDrawable.MapRenderLayer"/> the object is on. A higher
        /// layer depth results in the object being drawn on top of (in front of) objects with a lower value.
        /// </summary>
        [Browsable(false)]
        public int LayerDepth
        {
            get { return 0; }
        }

        /// <summary>
        /// Gets the <see cref="MapRenderLayer"/> that this object is rendered on.
        /// </summary>
        [Browsable(false)]
        public MapRenderLayer MapRenderLayer
        {
            get { return MapRenderLayer.SpriteForeground; }
        }

        /// <summary>
        /// Makes the object draw itself.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> the object can use to draw itself with.</param>
        public void Draw(ISpriteBatch sb)
        {
            if (BeforeDraw != null)
                BeforeDraw(this, sb);

            if (IsVisible)
            {
                var rect = ToRectangle();
                RenderRectangle.Draw(sb, rect, Color, Color.Black);
            }

            if (AfterDraw != null)
                AfterDraw(this, sb);
        }

        /// <summary>
        /// Checks if in the object is in view of the specified <paramref name="camera"/>.
        /// </summary>
        /// <param name="camera">The <see cref="ICamera2D"/> to check if this object is in view of.</param>
        /// <returns>
        /// True if the object is in view of the camera, else False.
        /// </returns>
        public bool InView(ICamera2D camera)
        {
            return camera.InView(this);
        }

        #endregion
    }
}