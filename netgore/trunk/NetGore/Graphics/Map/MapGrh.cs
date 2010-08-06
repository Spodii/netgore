using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using NetGore.IO;
using NetGore.World;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// A Grh instance bound to the map. This is simply a container for a map-bound Grh with no behavior
    /// besides rendering and updating, and resides completely on the Client.
    /// </summary>
    public class MapGrh : ISpatial, IDrawable, IPersistable
    {
        const string _distortionCategoryName = "Distortion";
        const string _grhIndexKeyName = "GrhIndex";
        const string _mapGrhCategoryName = "MapGrh";

        readonly Grh _grh;

        Color _color = Color.White;
        bool _isForeground;
        bool _isVisible = true;
        short _layerDepth;
        Vector2 _origin = Vector2.Zero;
        Vector2 _position;
        float _rotation = 0f;
        Vector2 _scale = Vector2.One;
        SpriteEffects _spriteEffects;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapGrh"/> class.
        /// </summary>
        /// <param name="grh">Grh to draw.</param>
        /// <param name="position">Position to draw on the map.</param>
        /// <param name="isForeground">If true, this will be drawn in the foreground layer. If false,
        /// it will be drawn in the background layer.</param>
        public MapGrh(Grh grh, Vector2 position, bool isForeground)
        {
            if (grh == null)
            {
                Debug.Fail("grh is null.");
                return;
            }

            _grh = grh;
            _position = position;
            IsForeground = isForeground;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapGrh"/> class.
        /// </summary>
        /// <param name="reader">The reader to read the values from.</param>
        /// <param name="currentTime">The current time.</param>
        public MapGrh(IValueReader reader, TickCount currentTime)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            _grh = new Grh(null, AnimType.Loop, currentTime);

            ReadState(reader);
        }

        /// <summary>
        /// Gets the <see cref="Grh"/> for the <see cref="MapGrh"/>.
        /// </summary>
        [Category(_mapGrhCategoryName)]
        [Browsable(true)]
        [DisplayName("GrhData")]
        [Description("The GrhData that is drawn for this MapGrh.")]
        public Grh Grh
        {
            get { return _grh; }
        }

        /// <summary>
        /// Gets or sets if the <see cref="MapGrh"/> is in the foreground layer, in front of characters and items.
        /// </summary>
        [Category(_mapGrhCategoryName)]
        [Browsable(true)]
        [DisplayName("IsForeground")]
        [Description("If the MapGrh is in the foreground layer, in front of characters and items.")]
        [DefaultValue(false)]
        [SyncValue]
        public bool IsForeground
        {
            get { return _isForeground; }
            set
            {
                if (_isForeground == value)
                    return;

                var oldLayer = MapRenderLayer;
                _isForeground = value;

                if (RenderLayerChanged != null)
                    RenderLayerChanged(this, oldLayer);
            }
        }

        /// <summary>
        /// Gets or sets the origin of the sprite in pixels, where (0,0) is the top-left corner of the sprite.
        /// </summary>
        [Category(_distortionCategoryName)]
        [Browsable(true)]
        [DisplayName("Origin")]
        [Description("The origin of the sprite, in pixels, where (0,0) is the top-left corner.")]
        [DefaultValue(typeof(Vector2), "0, 0")]
        [SyncValue]
        public Vector2 Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }

        /// <summary>
        /// Gets or sets the rotation of the sprite in radians.
        /// </summary>
        [Category(_distortionCategoryName)]
        [Browsable(true)]
        [DisplayName("Rotation")]
        [Description("The rotation of the sprite in radians.")]
        [DefaultValue(0f)]
        [SyncValue]
        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        /// <summary>
        /// Gets or sets the percent to scale the <see cref="MapGrh"/>, where <see cref="Vector2.One"/> is equal to the
        /// original size (no scaling). <see cref="Vector2.X"/> corresponds to scaling on the X axis, and <see cref="Vector2.Y"/>
        /// on the Y axis.
        /// </summary>
        [Category(_distortionCategoryName)]
        [Browsable(true)]
        [DisplayName("Scale")]
        [Description("The percent to scale the sprite on the X and Y axis.")]
        [DefaultValue(typeof(Vector2), "1, 1")]
        [SyncValue]
        public Vector2 Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="SpriteEffects"/> to apply to the sprite when drawing.
        /// </summary>
        [Category(_distortionCategoryName)]
        [Browsable(true)]
        [DisplayName("Effects")]
        [Description("The effects to apply to the sprite when drawing.")]
        [DefaultValue(SpriteEffects.None)]
        [SyncValue]
        public SpriteEffects SpriteEffects
        {
            get { return _spriteEffects; }
            set { _spriteEffects = value; }
        }

        /// <summary>
        /// Handles the actual drawing of the <see cref="MapGrh"/>.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> the object can use to draw itself with.</param>
        protected virtual void HandleDrawing(ISpriteBatch sb)
        {
            _grh.Draw(sb, Position, Color, SpriteEffects, Rotation, Origin, Scale);
        }

        /// <summary>
        /// Updates the <see cref="MapGrh"/>.
        /// </summary>
        /// <param name="currentTime">Current game time.</param>
        public virtual void Update(TickCount currentTime)
        {
            _grh.Update(currentTime);
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
        /// Notifies listeners when the <see cref="IDrawable.MapRenderLayer"/> property has changed.
        /// </summary>
        public event MapRenderLayerChange RenderLayerChanged;

        /// <summary>
        /// Notifies listeners when the <see cref="IDrawable.IsVisible"/> property has changed.
        /// </summary>
        public event IDrawableEventHandler VisibleChanged;

        /// <summary>
        /// Gets or sets the <see cref="IDrawable.Color"/> to use when drawing this <see cref="IDrawable"/>. By default, this
        /// value will be equal to white (ARGB: 255,255,255,255).
        /// </summary>
        [Category(_distortionCategoryName)]
        [Browsable(true)]
        [DisplayName("Color")]
        [Description(
            "The color to use when drawing this MapGrh. Using white (255,255,255,255) will draw with no color alterations.")]
        [DefaultValue(typeof(Color), "{255, 255, 255, 255}")]
        [SyncValue]
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
        /// The value must be between short.MinValue and short.MaxValue.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is less than short.MinValue
        /// or greater than short.MaxValue.</exception>
        [Category(_mapGrhCategoryName)]
        [Browsable(true)]
        [DisplayName("Layer Depth")]
        [Description("The drawing depth of the object. Objects with higher values are drawn above those with lower values.")]
        [DefaultValue((byte)0)]
        [SyncValue]
        public int LayerDepth
        {
            get { return _layerDepth; }
            set
            {
                if (value < short.MinValue || value > short.MaxValue)
                    throw new ArgumentOutOfRangeException("value", "value must be between short.MinValue and short.MaxValue.");

                _layerDepth = (short)value;
            }
        }

        /// <summary>
        /// Gets the <see cref="MapRenderLayer"/> that this object is rendered on.
        /// </summary>
        [Browsable(false)]
        public MapRenderLayer MapRenderLayer
        {
            get
            {
                // MapGrhs can be either foreground or background
                if (IsForeground)
                    return MapRenderLayer.SpriteForeground;
                else
                    return MapRenderLayer.SpriteBackground;
            }
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
                HandleDrawing(sb);

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
            return camera.InView(_grh, Position);
        }

        #endregion

        #region IPersistable Members

        /// <summary>
        /// Reads the state of the object from an <see cref="IValueReader"/>. Values should be read in the exact
        /// same order as they were written.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public virtual void ReadState(IValueReader reader)
        {
            var grhIndex = reader.ReadGrhIndex(_grhIndexKeyName);

            if (!grhIndex.IsInvalid)
                _grh.SetGrh(grhIndex);

            PersistableHelper.Read(this, reader);
        }

        /// <summary>
        /// Writes the state of the object to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        public virtual void WriteState(IValueWriter writer)
        {
            writer.Write(_grhIndexKeyName, Grh.GrhData != null ? Grh.GrhData.GrhIndex : GrhIndex.Invalid);

            PersistableHelper.Write(this, writer);
        }

        #endregion

        #region ISpatial Members

        /// <summary>
        /// Notifies listeners when this <see cref="ISpatial"/> has moved.
        /// </summary>
        public event SpatialEventHandler<Vector2> Moved;

        /// <summary>
        /// Unused by the <see cref="MapGrh"/>.
        /// </summary>
        event SpatialEventHandler<Vector2> ISpatial.Resized
        {
            add { }
            remove { }
        }

        /// <summary>
        /// Gets the center position of the <see cref="ISpatial"/>.
        /// </summary>
        [Browsable(false)]
        public Vector2 Center
        {
            get { return Position + (Size / 2); }
        }

        /// <summary>
        /// Gets the world coordinates of the bottom-right corner of this <see cref="ISpatial"/>.
        /// </summary>
        [Browsable(false)]
        public Vector2 Max
        {
            get { return Position + Size; }
        }

        /// <summary>
        /// Gets or sets the position to draw the <see cref="MapGrh"/> at.
        /// </summary>
        [Category(_mapGrhCategoryName)]
        [DisplayName("Position")]
        [Description("Location of the top-left corner of the MapGrh on the map.")]
        [Browsable(true)]
        [SyncValue]
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                if (Position == value)
                    return;

                var oldPosition = Position;
                _position = value;

                if (Moved != null)
                    Moved(this, oldPosition);
            }
        }

        /// <summary>
        /// Gets the size of this <see cref="ISpatial"/>.
        /// </summary>
        [Browsable(false)]
        public Vector2 Size
        {
            get { return _grh.Size * Scale; }
        }

        /// <summary>
        /// Gets a <see cref="Rectangle"/> that represents the world area that this <see cref="ISpatial"/> occupies.
        /// </summary>
        /// <returns>A <see cref="Rectangle"/> that represents the world area that this <see cref="ISpatial"/>
        /// occupies.</returns>
        public Rectangle ToRectangle()
        {
            return SpatialHelper.ToRectangle(this);
        }

        #endregion
    }
}