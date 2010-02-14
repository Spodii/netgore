using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Describes a single light.
    /// </summary>
    public class Light : ILight
    {
        Color _color;
        Vector2 _position;
        ISpatial _positionProvider;
        Vector2 _size;

        /// <summary>
        /// Handles when the <see cref="ILight.PositionProvider"/> moves.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        void PositionProvider_Moved(ISpatial sender, Vector2 e)
        {
            _position = sender.Position;
        }

        #region ILight Members

        /// <summary>
        /// Notifies listeners when this <see cref="ISpatial"/> has moved.
        /// </summary>
        public event SpatialEventHandler<Vector2> Moved;

        /// <summary>
        /// Notifies listeners when this <see cref="ISpatial"/> has been resized.
        /// </summary>
        public event SpatialEventHandler<Vector2> Resized;

        /// <summary>
        /// Gets the center position of the <see cref="ISpatial"/>.
        /// </summary>
        public Vector2 Center
        {
            get { return Position + (Size / 2f); }
        }

        /// <summary>
        /// Gets or sets the color of the light. The alpha value has no affect and will always be set to 255.
        /// </summary>
        public Color Color
        {
            get { return _color; }
            set { _color = new Color(value, 255); }
        }

        /// <summary>
        /// Gets the world coordinates of the bottom-right corner of this <see cref="ISpatial"/>.
        /// </summary>
        public Vector2 Max
        {
            get { return Position + Size; }
        }

        /// <summary>
        /// Gets the world coordinates of the top-left corner of this <see cref="ISpatial"/>.
        /// </summary>
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
                    Moved(this, oldValue);
            }
        }

        /// <summary>
        /// Gets or sets an <see cref="ISpatial"/> that provides the position to use. If set, the
        /// <see cref="ISpatial.Position"/> value will automatically be acquired with the position of the
        /// <see cref="ISpatial"/> instead, and setting the position will have no affect.
        /// </summary>
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
                    _positionProvider.Moved += PositionProvider_Moved;
                    _position = _positionProvider.Center;
                }
            }
        }

        /// <summary>
        /// Gets or sets the amount to rotate the <see cref="ILight"/> in radians.
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// Gets or sets the size of this <see cref="ILight"/>.
        /// </summary>
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
                    Resized(this, oldSize);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Grh"/> used to draw the light. If null, the light will not be drawn.
        /// </summary>
        public Grh Sprite { get; set; }

        /// <summary>
        /// Draws the <see cref="ILight"/>.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> to draw with.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Sprite == null)
                return;

            Sprite.Draw(spriteBatch, Position - (Size / 2f), Color, SpriteEffects.None, Rotation, Center, Size / Sprite.Size);
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
        /// Sets the size of this <see cref="ILight"/>.
        /// </summary>
        /// <param name="newSize">The new size.</param>
        public void Resize(Vector2 newSize)
        {
            Size = newSize;
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

        #endregion
    }
}