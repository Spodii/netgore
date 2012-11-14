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
    /// A single image that resides in the background of the map.
    /// </summary>
    public abstract class BackgroundImage : IDrawable
    {
        const string _valueKeyAlignment = "Alignment";
        const string _valueKeyColor = "Color";
        const string _valueKeyDepth = "Depth";
        const string _valueKeyGrhIndex = "GrhIndex";
        const string _valueKeyName = "Name";
        const string _valueKeyOffset = "Offset";

        readonly ICamera2DProvider _cameraProvider;
        readonly IMap _map;

        Color _color = Color.White;
        float _depth;
        bool _isVisible = true;
        string _name;
        Grh _sprite;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundImage"/> class.
        /// </summary>
        /// <param name="cameraProvider">The camera provider.</param>
        /// <param name="map">The map that this <see cref="BackgroundImage"/> is on.</param>
        /// <exception cref="ArgumentNullException"><paramref name="cameraProvider" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="map" /> is <c>null</c>.</exception>
        protected BackgroundImage(ICamera2DProvider cameraProvider, IMap map)
        {
            if (cameraProvider == null)
                throw new ArgumentNullException("cameraProvider");
            if (map == null)
                throw new ArgumentNullException("map");

            _cameraProvider = cameraProvider;
            _map = map;

            // Set the default values
            Offset = Vector2.Zero;
            Alignment = Alignment.TopLeft;

            _sprite = new Grh(null, AnimType.Loop, map.GetTime());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundImage"/> class.
        /// </summary>
        /// <param name="cameraProvider">The camera provider.</param>
        /// <param name="map">The map that this <see cref="BackgroundImage"/> is on.</param>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        /// <exception cref="ArgumentNullException"><paramref name="cameraProvider" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="map" /> is <c>null</c>.</exception>
        protected BackgroundImage(ICamera2DProvider cameraProvider, IMap map, IValueReader reader)
        {
            if (cameraProvider == null)
                throw new ArgumentNullException("cameraProvider");
            if (map == null)
                throw new ArgumentNullException("map");

            _cameraProvider = cameraProvider;
            _map = map;

            Read(reader);
        }

        /// <summary>
        /// Gets or sets how the background image is aligned to the map. Default is TopLeft.
        /// </summary>
        [Category("Position")]
        [DisplayName("Alignment")]
        [Description("How the background image is aligned to the map.")]
        [DefaultValue(Alignment.TopLeft)]
        [Browsable(true)]
        public Alignment Alignment { get; set; }

        /// <summary>
        /// Gets the <see cref="ICamera2D"/> used to view the <see cref="BackgroundImage"/>.
        /// </summary>
        [Browsable(false)]
        protected ICamera2D Camera
        {
            get { return _cameraProvider.Camera; }
        }

        /// <summary>
        /// Gets or sets the depth of the image relative to other background images, and how fast the
        /// image moves with the camera. A depth of 1.0 will move as fast as the camera, while a depth of
        /// 2.0 will move at half the speed of the camera. Must be greater than or equal to 1.0. Default is 1.0.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is less than 1.0.</exception>
        [Category("Position")]
        [DisplayName("Depth")]
        [Description(
            "Defines the drawing order and movement speed, where 1.0 is same speed of the camera, and 2.0 is half the speed.")]
        [DefaultValue(1)]
        [Browsable(true)]
        public float Depth
        {
            get { return _depth; }
            set
            {
                if (value < 1.0f)
                    throw new ArgumentOutOfRangeException("value");

                _depth = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="IMap"/> the <see cref="BackgroundImage"/> is on.
        /// </summary>
        [Browsable(false)]
        protected IMap Map
        {
            get { return _map; }
        }

        /// <summary>
        /// Gets or sets the optional name of this BackgroundImage. This is only used for personal purposes and
        /// has absolutely no affect on the BackgroundImage itself.
        /// </summary>
        [Category("Design")]
        [DisplayName("Name")]
        [Description("The optional name of this BackgroundImage. Used for development purposes only.")]
        [Browsable(true)]
        public string Name
        {
            get { return _name; }
            set { _name = value ?? string.Empty; }
        }

        /// <summary>
        /// Gets or sets the pixel offset of the image from the Alignment.
        /// </summary>
        [Category("Position")]
        [DisplayName("Offset")]
        [Description("The pixel offset of the image from the Alignment.")]
        [DefaultValue(typeof(Vector2), "0, 0")]
        [Browsable(true)]
        public Vector2 Offset { get; set; }

        /// <summary>
        /// Gets the sprite to draw. 
        /// </summary>
        [Category("Display")]
        [DisplayName("Sprite")]
        [Description("The sprite to draw.")]
        [Browsable(true)]
        public Grh Sprite
        {
            get { return _sprite; }
        }

        /// <summary>
        /// Gets the size of the Sprite source image.
        /// </summary>
        [Browsable(false)]
        protected Vector2 SpriteSourceSize
        {
            get
            {
                if (!IsSpriteSet())
                    return Vector2.Zero;

                return new Vector2(Sprite.Source.Width, Sprite.Source.Height);
            }
        }

        /// <summary>
        /// Gets the multiplier to use to offset an <see cref="Alignment"/>.
        /// </summary>
        /// <param name="alignment">The <see cref="Alignment"/>.</param>
        /// <returns>The offset to use for the <paramref name="alignment"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="alignment"/> contains a value not defined by the
        /// <see cref="Alignment"/> enum.</exception>
        static Vector2 GetOffsetMultiplier(Alignment alignment)
        {
            switch (alignment)
            {
                case Alignment.TopLeft:
                    return new Vector2(0, 0);
                case Alignment.TopRight:
                    return new Vector2(1, 0);
                case Alignment.BottomLeft:
                    return new Vector2(0, 1);
                case Alignment.BottomRight:
                    return new Vector2(1, 1);
                case Alignment.Top:
                    return new Vector2(0.5f, 0);
                case Alignment.Bottom:
                    return new Vector2(0.5f, 1f);
                case Alignment.Left:
                    return new Vector2(0, 0.5f);
                case Alignment.Right:
                    return new Vector2(1, 0.5f);
                case Alignment.Center:
                    return new Vector2(0.5f, 0.5f);
                default:
                    throw new ArgumentOutOfRangeException("alignment");
            }
        }

        /// <summary>
        /// Finds the map position of the image using the given <paramref name="camera"/>.
        /// </summary>
        /// <param name="mapSize">Size of the map that this image is on.</param>
        /// <param name="camera">Camera that describes the current view.</param>
        /// <param name="spriteSize">Size of the Sprite that will be drawn.</param>
        /// <returns>The map position of the image using the given <paramref name="camera"/>.</returns>
        protected Vector2 GetPosition(Vector2 mapSize, ICamera2D camera, Vector2 spriteSize)
        {
            // Can't draw a sprite that has no size...
            if (spriteSize == Vector2.Zero)
                return Vector2.Zero;

            // Get the position from the alignment
            var alignmentPosition = AlignmentHelper.FindOffset(Alignment, spriteSize, mapSize);

            // Add the custom offset
            var position = alignmentPosition + Offset;

            // Find the difference between the position and the camera's min position
            var diff = camera.Min - position;

            // Use the multiplier to align it to the correct part of the camera
            diff += (camera.Size - spriteSize) * GetOffsetMultiplier(Alignment);

            // Compensate for the depth
            diff *= ((1 / Depth) - 1);

            // Add the difference to the position
            position -= diff;

            return position;
        }

        /// <summary>
        /// Finds the map position of the image using the given <paramref name="camera"/>.
        /// </summary>
        /// <param name="mapSize">Size of the map that this image is on.</param>
        /// <param name="camera">Camera that describes the current view.</param>
        /// <returns>The map position of the image using the given <paramref name="camera"/>.</returns>
        protected Vector2 GetPosition(Vector2 mapSize, ICamera2D camera)
        {
            return GetPosition(mapSize, camera, SpriteSourceSize);
        }

        /// <summary>
        /// Handles drawing the <see cref="BackgroundImage"/>.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        protected virtual void HandleDraw(ISpriteBatch spriteBatch)
        {
            var position = GetPosition(Map.Size, Camera);
            Sprite.Draw(spriteBatch, position, Color);
        }

        /// <summary>
        /// Gets the <see cref="BackgroundImage.LayerDepth"/> value from the <see cref="IDrawable.LayerDepth"/>.
        /// </summary>
        /// <param name="imageDepth">The <see cref="BackgroundImage.LayerDepth"/> value.</param>
        /// <returns>The <see cref="IDrawable.LayerDepth"/> value for the <paramref name="imageDepth"/>.</returns>
        public static int ImageDepthToLayerDepth(float imageDepth)
        {
            return (int)(imageDepth * -100);
        }

        /// <summary>
        /// Gets if the Sprite is set and valid.
        /// </summary>
        /// <returns>True if the Sprite is set; otherwise false.</returns>
        protected virtual bool IsSpriteSet()
        {
            return Sprite != null && Sprite.GrhData != null;
        }

        /// <summary>
        /// Gets the <see cref="IDrawable.LayerDepth"/> value from the <see cref="BackgroundImage.LayerDepth"/>.
        /// </summary>
        /// <param name="layerDepth">The <see cref="IDrawable.LayerDepth"/> value.</param>
        /// <returns>The <see cref="BackgroundImage.LayerDepth"/> value for the <paramref name="layerDepth"/>.</returns>
        public static float LayerDepthToImageDepth(int layerDepth)
        {
            return layerDepth * -0.01f;
        }

        /// <summary>
        /// Reads the <see cref="BackgroundImage"/> from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read from.</param>
        protected virtual void Read(IValueReader reader)
        {
            Name = reader.ReadString(_valueKeyName);
            Alignment = reader.ReadEnum<Alignment>(_valueKeyAlignment);
            Color = reader.ReadColor(_valueKeyColor);
            Depth = reader.ReadFloat(_valueKeyDepth);
            Offset = reader.ReadVector2(_valueKeyOffset);
            var grhIndex = reader.ReadGrhIndex(_valueKeyGrhIndex);

            _sprite = new Grh(grhIndex, AnimType.Loop, Map.GetTime());
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            string name;
            if (string.IsNullOrEmpty(Name))
                name = "Unnamed";
            else
                name = Name;

            return string.Format("[{0}] {1}", GetType().Name, name);
        }

        /// <summary>
        /// Updates the <see cref="BackgroundImage"/>.
        /// </summary>
        /// <param name="currentTime">Current game time.</param>
        public virtual void Update(TickCount currentTime)
        {
            if (!IsSpriteSet())
                return;

            Sprite.Update(currentTime);
        }

        /// <summary>
        /// Writes the <see cref="BackgroundImage"/> to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        public virtual void Write(IValueWriter writer)
        {
            writer.Write(_valueKeyName, Name);
            writer.WriteEnum(_valueKeyAlignment, Alignment);
            writer.Write(_valueKeyColor, Color);
            writer.Write(_valueKeyDepth, Depth);
            writer.Write(_valueKeyOffset, Offset);

            GrhIndex grhIndex;
            if (IsSpriteSet())
                grhIndex = Sprite.GrhData.GrhIndex;
            else
            {
                grhIndex = GrhIndex.Invalid;
                Debug.Fail("Why is the Sprite not set? That doesn't seem right...");
            }

            writer.Write(_valueKeyGrhIndex, grhIndex);
        }

        #region IDrawable Members

        /// <summary>
        /// Notifies listeners immediately after this <see cref="IDrawable"/> is drawn.
        /// This event will be raised even if <see cref="IDrawable.IsVisible"/> is false.
        /// </summary>
        public event TypedEventHandler<IDrawable, EventArgs<ISpriteBatch>> AfterDraw;

        /// <summary>
        /// Notifies listeners immediately before this <see cref="IDrawable"/> is drawn.
        /// This event will be raised even if <see cref="IDrawable.IsVisible"/> is false.
        /// </summary>
        public event TypedEventHandler<IDrawable, EventArgs<ISpriteBatch>> BeforeDraw;

        /// <summary>
        /// Notifies listeners when the <see cref="IDrawable.Color"/> property has changed.
        /// </summary>
        public event TypedEventHandler<IDrawable> ColorChanged;

        /// <summary>
        /// Unused by the <see cref="BackgroundImage"/> since the layer never changes.
        /// </summary>
        event TypedEventHandler<IDrawable, ValueChangedEventArgs<MapRenderLayer>> IDrawable.RenderLayerChanged
        {
            add { }
            remove { }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="IDrawable.IsVisible"/> property has changed.
        /// </summary>
        public event TypedEventHandler<IDrawable> VisibleChanged;

        /// <summary>
        /// Gets or sets the <see cref="IDrawable.Color"/> to use when drawing this <see cref="IDrawable"/>. By default, this
        /// value will be equal to white (ARGB: 255,255,255,255).
        /// </summary>
        [Category("Display")]
        [DisplayName("Color")]
        [Description("The color to use when drawing the image where RGBA 255,255,255,255 will draw the image unaltered.")]
        [DefaultValue(typeof(Color), "255, 255, 255, 255")]
        [Browsable(true)]
        public Color Color
        {
            get { return _color; }
            set
            {
                if (_color == value)
                    return;

                _color = value;

                if (ColorChanged != null)
                    ColorChanged.Raise(this, EventArgs.Empty);
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
                    VisibleChanged.Raise(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets the depth of the object for the <see cref="IDrawable.MapRenderLayer"/> the object is on. A higher
        /// layer depth results in the object being drawn on top of (in front of) objects with a lower value.
        /// </summary>
        [Browsable(false)]
        public int LayerDepth
        {
            get { return ImageDepthToLayerDepth(Depth); }
        }

        /// <summary>
        /// Gets the <see cref="IDrawable.MapRenderLayer"/> that this object is rendered on.
        /// </summary>
        [Browsable(false)]
        public MapRenderLayer MapRenderLayer
        {
            get { return MapRenderLayer.Background; }
        }

        /// <summary>
        /// Makes the object draw itself.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> the object can use to draw itself with.</param>
        public void Draw(ISpriteBatch sb)
        {
            // Ensure the sprite is set
            if (!IsSpriteSet())
                return;

            // Pre-drawing
            if (BeforeDraw != null)
                BeforeDraw.Raise(this, EventArgsHelper.Create(sb));

            // Draw
            if (IsVisible)
                HandleDraw(sb);

            // Post-drawing
            if (AfterDraw != null)
                AfterDraw.Raise(this, EventArgsHelper.Create(sb));
        }

        /// <summary>
        /// Checks if in the object is in view of the specified <paramref name="camera"/>.
        /// </summary>
        /// <param name="camera">The <see cref="ICamera2D"/> to check if this object is in view of.</param>
        /// <returns>True if the object is in view of the camera, else False.</returns>
        public bool InView(ICamera2D camera)
        {
            if (!IsSpriteSet())
                return false;

            // FUTURE: Implement properly
            return true;

            /*
            Vector2 position = GetPosition(Map.Size, Camera);
            if (position.X > camera.Max.X || position.Y > camera.Max.Y)
                return false;

            Vector2 max = position + Sprite.Size;
            if (max.X < camera.Min.X || max.Y < camera.Min.Y)
                return false;

            return true;
            */
        }

        #endregion

        /// <summary>
        /// Not supported by BackgroundImage. Always returns Vector2.Zero.
        /// </summary>
        Vector2 IPositionable.Position { get { return Vector2.Zero; } }

        /// <summary>
        /// Not supported by BackgroundImage. Always returns Vector2.Zero.
        /// </summary>
        Vector2 IPositionable.Size { get { return Vector2.Zero; } }
    }
}