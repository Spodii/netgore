using System.ComponentModel;
using System.Linq;
using NetGore.IO;
using NetGore.World;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Describes a single light.
    /// </summary>
    public class Light : ILight
    {
        const string _colorValueKey = "Color";
        const string _isEnabledValueKey = "IsEnabled";
        const string _positionValueKey = "Position";
        const string _rotationValueKey = "Rotation";
        const string _sizeValueKey = "Size";
        const string _spriteValueKey = "Sprite";

        Color _color = Color.White;
        Vector2 _position;
        ISpatial _positionProvider;
        Vector2 _size = new Vector2(128);

        /// <summary>
        /// Initializes a new instance of the <see cref="Light"/> class.
        /// </summary>
        public Light()
        {
            IsEnabled = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Light"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the initial light state from.</param>
        public Light(IValueReader reader)
        {
            ReadState(reader);
        }

        /// <summary>
        /// Handles when the <see cref="ILight.PositionProvider"/> moves.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{Vector2}"/> instance containing the event data.</param>
        void PositionProvider_Moved(ISpatial sender, EventArgs<Vector2> e)
        {
            _position = sender.Position;
        }

        #region ILight Members

        /// <summary>
        /// Notifies listeners when this <see cref="ISpatial"/> has moved.
        /// </summary>
        public event TypedEventHandler<ISpatial, EventArgs<Vector2>> Moved;

        /// <summary>
        /// Notifies listeners when this <see cref="ISpatial"/> has been resized.
        /// </summary>
        public event TypedEventHandler<ISpatial, EventArgs<Vector2>> Resized;

        /// <summary>
        /// Gets or sets the center position of the <see cref="ISpatial"/>.
        /// </summary>
        [Browsable(false)]
        public Vector2 Center
        {
            get { return Position + (Size / 2f); }
            set { Position = value - (Size / 2f); }
        }

        /// <summary>
        /// Gets or sets the color of the light. The alpha value has no affect and will always be set to 255.
        /// </summary>
        [DisplayName("Color")]
        [Description("The color of the light. The alpha value has no affect and will always be set to 255.")]
        [DefaultValue(typeof(Color), "{255, 255, 255, 255}")]
        [Browsable(true)]
        public Color Color
        {
            get { return _color; }
            set { _color = new Color(value.R, value.G, value.B, 255); }
        }

        /// <summary>
        /// Gets or sets if this light is enabled.
        /// </summary>
        [DisplayName("Enabled")]
        [Description("If this light is enabled.")]
        [DefaultValue(true)]
        [Browsable(true)]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Gets the world coordinates of the bottom-right corner of this <see cref="ISpatial"/>.
        /// </summary>
        [Browsable(false)]
        public Vector2 Max
        {
            get { return Position + Size; }
        }

        /// <summary>
        /// Gets the world coordinates of the top-left corner of this <see cref="ISpatial"/>.
        /// </summary>
        [DisplayName("Position")]
        [Description("The world position of the light.")]
        [Browsable(true)]
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                if (_position == value)
                    return;

                var oldValue = _position;
                _position = value;

                if (Moved != null)
                    Moved.Raise(this, EventArgsHelper.Create(oldValue));
            }
        }

        /// <summary>
        /// Gets or sets an <see cref="ISpatial"/> that provides the position to use. If set, the
        /// <see cref="ISpatial.Position"/> value will automatically be acquired with the position of the
        /// <see cref="ISpatial"/> instead, and setting the position will have no affect.
        /// </summary>
        [Browsable(false)]
        public ISpatial PositionProvider
        {
            get { return _positionProvider; }
            set
            {
                if (_positionProvider == value)
                    return;

                if (_positionProvider != null)
                    _positionProvider.Moved -= PositionProvider_Moved;

                _positionProvider = value;

                if (_positionProvider != null)
                {
                    _positionProvider.Moved -= PositionProvider_Moved;
                    _positionProvider.Moved += PositionProvider_Moved;
                    _position = _positionProvider.Center;
                }
            }
        }

        /// <summary>
        /// Gets or sets the amount to rotate the <see cref="ILight"/> in radians.
        /// </summary>
        [DisplayName("Rotation")]
        [Description("The amount to rotate the light in radians.")]
        [DefaultValue(0f)]
        [Browsable(true)]
        public float Rotation { get; set; }

        /// <summary>
        /// Gets or sets the size of this <see cref="ILight"/>.
        /// </summary>
        [DisplayName("Size")]
        [Description("The size of the light.")]
        [Browsable(true)]
        public Vector2 Size
        {
            get { return _size; }

            set
            {
                if (_size == value)
                    return;

                var oldSize = _size;
                _size = value;

                if (Resized != null)
                    Resized.Raise(this, EventArgsHelper.Create(oldSize));
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Grh"/> used to draw the light. If null, the light will not be drawn.
        /// </summary>
        [DisplayName("Sprite")]
        [Description("The sprite used to draw the light.")]
        [Browsable(true)]
        public Grh Sprite { get; set; }

        /// <summary>
        /// Gets if this <see cref="ISpatial"/> can ever be moved with <see cref="ISpatial.TryMove"/>.
        /// </summary>
        bool ISpatial.SupportsMove
        {
            get { return true; }
        }

        /// <summary>
        /// Gets if this <see cref="ISpatial"/> can ever be resized with <see cref="ISpatial.TryResize"/>.
        /// </summary>
        bool ISpatial.SupportsResize
        {
            get { return true; }
        }

        /// <summary>
        /// Gets or sets an object that can be used to identify or store information about this <see cref="ILight"/>.
        /// This property is purely optional.
        /// </summary>
        [Browsable(false)]
        public object Tag { get; set; }

        /// <summary>
        /// Draws the <see cref="ILight"/>.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw with.</param>
        public void Draw(ISpriteBatch spriteBatch)
        {
            // Skip when not enabled
            if (!IsEnabled)
                return;

            // Make sure we have a valid sprite
            var s = Sprite;
            if (s == null)
                return;

            // Draw
            var scale = Size / s.Size;
            s.Draw(spriteBatch, Position, Color, SpriteEffects.None, Rotation, scale / 2f, scale);
        }

        /// <summary>
        /// Translates this <see cref="ILight"/> relative to the current position.
        /// </summary>
        /// <param name="offset">The amount to move from the current position.</param>
        public void Move(Vector2 offset)
        {
            Position += offset;
        }

        /// <summary>
        /// Reads the state of the object from an <see cref="IValueReader"/>. Values should be read in the exact
        /// same order as they were written.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public void ReadState(IValueReader reader)
        {
            PositionProvider = null;
            Tag = null;

            Position = reader.ReadVector2(_positionValueKey);
            Size = reader.ReadVector2(_sizeValueKey);
            Color = reader.ReadColor(_colorValueKey);
            Rotation = reader.ReadFloat(_rotationValueKey);
            IsEnabled = reader.ReadBool(_isEnabledValueKey);

            var grhIndex = reader.ReadGrhIndex(_spriteValueKey);
            if (!grhIndex.IsInvalid)
                Sprite = new Grh(grhIndex, AnimType.Loop, 0);
            else
                Sprite = null;
        }

        /// <summary>
        /// Sets the size of this <see cref="ILight"/>.
        /// </summary>
        /// <param name="newSize">The new size.</param>
        public void Resize(Vector2 newSize)
        {
            Size = newSize;
        }

        /// <summary>
        /// Sets the center position of the <see cref="ILight"/>.
        /// </summary>
        /// <param name="value">The new center.</param>
        public void SetCenter(Vector2 value)
        {
            Center = value;
        }

        /// <summary>
        /// Moves this <see cref="ILight"/> to a new position.
        /// </summary>
        /// <param name="newPosition">The new position.</param>
        public void Teleport(Vector2 newPosition)
        {
            Position = newPosition;
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

        /// <summary>
        /// Tries to move the <see cref="ISpatial"/>.
        /// </summary>
        /// <param name="newPos">The new position.</param>
        /// <returns>True if the <see cref="ISpatial"/> was moved to the <paramref name="newPos"/>; otherwise false.</returns>
        bool ISpatial.TryMove(Vector2 newPos)
        {
            Position = newPos;
            return true;
        }

        /// <summary>
        /// Tries to resize the <see cref="ISpatial"/>.
        /// </summary>
        /// <param name="newSize">The new size.</param>
        /// <returns>True if the <see cref="ISpatial"/> was resized to the <paramref name="newSize"/>; otherwise false.</returns>
        bool ISpatial.TryResize(Vector2 newSize)
        {
            Size = newSize;
            return true;
        }

        /// <summary>
        /// Updates the <see cref="ILight"/>.
        /// </summary>
        /// <param name="currentTime">The current game time in milliseconds.</param>
        public void Update(TickCount currentTime)
        {
            // Skip when not enabled
            if (!IsEnabled)
                return;

            // Update our sprite (if we have one)
            var s = Sprite;
            if (s != null)
                s.Update(currentTime);
        }

        /// <summary>
        /// Writes the state of the object to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        public void WriteState(IValueWriter writer)
        {
            writer.Write(_positionValueKey, Position);
            writer.Write(_sizeValueKey, Size);
            writer.Write(_colorValueKey, Color);
            writer.Write(_rotationValueKey, Rotation);
            writer.Write(_isEnabledValueKey, IsEnabled);
            writer.Write(_spriteValueKey, Sprite != null ? Sprite.GrhData.GrhIndex : GrhIndex.Invalid);
        }

        #endregion
    }
}