using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore.IO;

namespace NetGore.Graphics
{
    /// <summary>
    /// A Grh instance bound to the map. This is simply a container for a map-bound Grh with no behavior
    /// besides rendering and updating, and resides completely on the Client.
    /// </summary>
    public class MapGrh : ISpatial, IDrawable
    {
        readonly Grh _grh;

        bool _isForeground;
        Vector2 _position;

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
        public MapGrh(IValueReader reader, int currentTime)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            var position = reader.ReadVector2("Position");
            GrhIndex grhIndex = reader.ReadGrhIndex("GrhIndex");
            _isForeground = reader.ReadBool("IsForeground");

            _grh = new Grh(grhIndex, AnimType.Loop, currentTime);
            _position = position;
        }

        /// <summary>
        /// Gets the <see cref="Grh"/> for the <see cref="MapGrh"/>.
        /// </summary>
        public Grh Grh
        {
            get { return _grh; }
        }

        /// <summary>
        /// Gets or sets if the <see cref="MapGrh"/> is in the foreground.
        /// </summary>
        public bool IsForeground
        {
            get { return _isForeground; }
            set
            {
                if (_isForeground == value)
                    return;

                MapRenderLayer oldLayer = MapRenderLayer;
                _isForeground = value;

                if (OnChangeRenderLayer != null)
                    OnChangeRenderLayer(this, oldLayer);
            }
        }

        /// <summary>
        /// Updates the <see cref="MapGrh"/>.
        /// </summary>
        /// <param name="currentTime">Current game time.</param>
        public void Update(int currentTime)
        {
            _grh.Update(currentTime);
        }

        /// <summary>
        /// Writes the MapGrh to an IValueWriter.
        /// </summary>
        /// <param name="writer">IValueWriter to write the MapGrh to.</param>
        public void Write(IValueWriter writer)
        {
            writer.Write("Position", Position);
            writer.Write("GrhIndex", Grh.GrhData.GrhIndex);
            writer.Write("IsForeground", IsForeground);
        }

        #region IDrawable Members

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

        /// <summary>
        /// Gets the <see cref="MapRenderLayer"/> that this object is rendered on.
        /// </summary>
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
        /// Notifies listeners that the object's <see cref="MapRenderLayer"/> has changed.
        /// </summary>
        public event MapRenderLayerChange OnChangeRenderLayer;

        /// <summary>
        /// Makes the object draw itself.
        /// </summary>
        /// <param name="sb"><see cref="SpriteBatch"/> the object can use to draw itself with.</param>
        public void Draw(SpriteBatch sb)
        {
            _grh.Draw(sb, Position, Color.White);
        }

        #endregion

        #region ISpatial Members

        /// <summary>
        /// Gets a <see cref="Rectangle"/> that represents the world area that this <see cref="ISpatial"/> occupies.
        /// </summary>
        /// <returns>A <see cref="Rectangle"/> that represents the world area that this <see cref="ISpatial"/>
        /// occupies.</returns>
        public Rectangle ToRectangle()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
        }

        /// <summary>
        /// Gets or sets the position to draw the <see cref="MapGrh"/> at.
        /// </summary>
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                if (Position == value)
                    return;

                var oldPosition = Position;
                _position = value;

                if (OnMove != null)
                    OnMove(this, oldPosition);
            }
        }

        /// <summary>
        /// Gets the size of this <see cref="ISpatial"/>.
        /// </summary>
        public Vector2 Size
        {
            get { return _grh.Size; }
        }

        /// <summary>
        /// Gets the world coordinates of the bottom-right corner of this <see cref="ISpatial"/>.
        /// </summary>
        public Vector2 Max
        {
            get { return Position + Size; }
        }

        /// <summary>
        /// Notifies listeners when this <see cref="ISpatial"/> has moved.
        /// </summary>
        public event SpatialMoveEventHandler OnMove;

        /// <summary>
        /// Notifies listeners when this <see cref="ISpatial"/> has been resized.
        /// </summary>
        public event SpatialResizeEventHandler OnResize;

        #endregion
    }
}