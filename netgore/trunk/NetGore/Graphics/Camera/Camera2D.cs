using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.World;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Describes a camera for a 2D world.
    /// </summary>
    public class Camera2D : ICamera2D
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        bool _keepInMap = true;
        IMap _map;
        Vector2 _min = Vector2.Zero;
        float _rotation = 0.0f;
        float _scale = 1.0f;
        Vector2 _size;

        /// <summary>
        /// Initializes a new instance of the <see cref="Camera2D"/> class.
        /// </summary>
        /// <param name="screenSize">Size of the screen in pixels.</param>
        public Camera2D(Vector2 screenSize)
        {
            _size = screenSize;
        }

        void ApplyKeepInMap()
        {
            if (!KeepInMap || Map == null)
                return;

            // Check the max values
            var max = Max;
            if (max.X > Map.Width)
                _min.X = Map.Width - Size.X;

            if (max.Y > Map.Height)
                _min.Y = Map.Height - Size.Y;

            // Check the min values
            if (_min.X < 0)
                _min.X = 0;
            if (_min.Y < 0)
                _min.Y = 0;
        }

        #region ICamera2D Members

        /// <summary>
        /// Gets the coordinate of the center of the <see cref="ICamera2D"/>'s view port.
        /// </summary>
        public Vector2 Center
        {
            get { return Min + (Size / 2f); }
        }

        /// <summary>
        /// Gets or sets if the camera is forced to stay in view of the map. If true, the camera will never show anything
        /// outside of the range of the map. Only valid if <see cref="ICamera2D.Map"/> is not null.
        /// </summary>
        public bool KeepInMap
        {
            get { return _keepInMap; }
            set
            {
                if (_keepInMap == value)
                    return;

                _keepInMap = value;

                ApplyKeepInMap();
            }
        }

        /// <summary>
        /// Gets or sets the map that this camera is viewing.
        /// </summary>
        public IMap Map
        {
            get { return _map; }
            set
            {
                _map = value;
                ApplyKeepInMap();
            }
        }

        /// <summary>
        /// Gets the coordinates of the bottom-right corner of the camera's visible area.
        /// </summary>
        public Vector2 Max
        {
            get { return Min + Size; }
        }

        /// <summary>
        /// Gets or sets the coordinates of the center of the top-left corner of the camera's visible area.
        /// </summary>
        public Vector2 Min
        {
            get { return _min; }
            set
            {
                // Check that the value is different
                if (_min == value)
                    return;

                // Update the value and the matrix
                _min = value;

                ApplyKeepInMap();
            }
        }

        /// <summary>
        /// Gets or sets the camera's rotation magnitude in radians.
        /// </summary>
        public float Rotation
        {
            get { return _rotation; }
            set
            {
                // Check that the value is different
                if (_rotation == value)
                    return;

                // Update the value and the matrix
                _rotation = value;
            }
        }

        /// <summary>
        /// Gets or sets the camera scale percent where 1 equals 100%, or no scaling.
        /// </summary>
        public float Scale
        {
            get { return _scale; }
            set
            {
                // Check that the value is different
                if (_scale == value)
                    return;

                if (value < 1)
                    return;

                // Update the value and the matrix
                var oldSizeUnscaled = _size * _scale;
                _scale = value;
                _size = oldSizeUnscaled / _scale;

                ApplyKeepInMap();
            }
        }

        /// <summary>
        /// Gets or sets the size of the camera's view area in pixels, taking the <see cref="ICamera2D.Scale"/> value
        /// into consideration.
        /// </summary>
        public Vector2 Size
        {
            get { return _size; }
            set { _size = value / Scale; }
        }

        /// <summary>
        /// Centers the camera on an <see cref="Entity"/> so that the center of the <see cref="Entity"/> is at the center
        /// of the camera.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> to center on.</param>
        /// <exception cref="ArgumentNullException"><paramref name="entity" /> is <c>null</c>.</exception>
        public void CenterOn(Entity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            CenterOn(entity.Position + (entity.Size / 2f));
        }

        /// <summary>
        /// Centers the camera on a point so that the point is at the center of the camera.
        /// </summary>
        /// <param name="point">Point to center on.</param>
        public void CenterOn(Vector2 point)
        {
            Min = point - (Size / 2f);
        }

        /// <summary>
        /// Gets a <see cref="Rectangle"/> that describes the region of the world area visible by the camera.
        /// </summary>
        /// <returns>A <see cref="Rectangle"/> that describes the region of the world area visible by the camera.</returns>
        public Rectangle GetViewArea()
        {
            var min = Min;
            var max = Max;

            Vector2 size;
            Vector2.Subtract(ref max, ref min, out size);

            return new Rectangle(min.X, min.Y, size.X, size.Y);
        }

        /// <summary>
        /// Checks if a specified object is in view of the camera.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/> to check if in view.</param>
        /// <returns>
        /// True if in the view area; otherwise false.
        /// </returns>
        public bool InView(ISpatial spatial)
        {
            if (spatial == null)
            {
                const string errmsg = "spatial is null.";
                Debug.Fail(errmsg);
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                return false;
            }

            return InView(spatial.Position, spatial.Size);
        }

        /// <summary>
        /// Checks if a specified object is in view of the camera.
        /// </summary>
        /// <param name="grh">The <see cref="Grh"/> to check.</param>
        /// <param name="position">The position of the <see cref="Grh"/>.</param>
        /// <returns>True if in the view area; otherwise false.</returns>
        public bool InView(Grh grh, Vector2 position)
        {
            if (grh == null)
            {
                const string errmsg = "grh is null.";
                Debug.Fail(errmsg);
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                return false;
            }

            return InView(position, grh.Size);
        }

        /// <summary>
        /// Checks if a specified object is in view of the screen.
        /// </summary>
        /// <param name="position">Position of the object.</param>
        /// <param name="size">The size of the object.</param>
        /// <returns>True if in the view area, else false.</returns>
        public bool InView(Vector2 position, Vector2 size)
        {
            return (position.X < Max.X) && (position.Y < Max.Y) && (position.X + size.X > Min.X) && (position.Y + size.Y > Min.Y);
        }

        /// <summary>
        /// Checks if a specified <see cref="Rectangle"/> is in view of the camera.
        /// </summary>
        /// <param name="rect">The <see cref="Rectangle"/> to check.</param>
        /// <returns>True if in the view area; otherwise false.</returns>
        public bool InView(Rectangle rect)
        {
            return InView(new Vector2(rect.X, rect.Y), new Vector2(rect.Width, rect.Height));
        }

        /// <summary>
        /// Translates a world position to a screen position.
        /// </summary>
        /// <param name="worldPosition">The world position.</param>
        /// <returns>The screen position for the given <see cref="worldPosition"/>.</returns>
        public Vector2 ToScreen(Vector2 worldPosition)
        {
            return (worldPosition - Min) * Scale;
        }

        /// <summary>
        /// Translates a world position to a screen position.
        /// </summary>
        /// <param name="worldX">The world position X co-ordinate.</param>
        /// <param name="worldY">The world position Y co-ordinate.</param>
        /// <returns>The screen position for the given <see cref="worldX"/> and <see cref="worldY"/>.</returns>
        public Vector2 ToScreen(float worldX, float worldY)
        {
            return ToScreen(new Vector2(worldX, worldY));
        }

        /// <summary>
        /// Translates a screen position to world position.
        /// </summary>
        /// <param name="screenPosition">The screen position.</param>
        /// <returns>The world position for the given <see cref="screenPosition"/>.</returns>
        public Vector2 ToWorld(Vector2 screenPosition)
        {
            return (screenPosition / Scale) + Min;
        }

        /// <summary>
        /// Translates a screen position to world position.
        /// </summary>
        /// <param name="screenX">The screen position X co-ordinate.</param>
        /// <param name="screenY">The screen position Y co-ordinate.</param>
        /// <returns>The world position for the given <see cref="screenX"/> and <see cref="screenY"/>.</returns>
        public Vector2 ToWorld(float screenX, float screenY)
        {
            return ToWorld(new Vector2(screenX, screenY));
        }

        /// <summary>
        /// Moves the camera's position using the given <paramref name="offset"/>.
        /// </summary>
        /// <param name="offset">The amount and direction to move the camera.</param>
        public void Translate(Vector2 offset)
        {
            Min += offset;
        }

        /// <summary>
        /// Zooms the camera in and focuses on a specified point.
        /// </summary>
        /// <param name="origin">Point to zoom in on (the center of the view).</param>
        /// <param name="size">The original size of the camera view.</param>
        /// <param name="scale">Magnification scale in percent. Must be non-zero.</param>
        /// <exception cref="ArgumentException"><paramref name="scale"/> is equal to 0.</exception>
        public void Zoom(Vector2 origin, Vector2 size, float scale)
        {
            Min = origin - size / (2f * scale);
            Scale = scale;
        }

        /// <summary>
        /// Finds the position for the given area to keep it in full view (or as much as possible) of the camera.
        /// </summary>
        /// <param name="screenArea">The area to keep in the screen (screen position).</param>
        /// <returns>The position for the screenArea to keep it in screen.</returns>
        public Vector2 ClampScreenPosition(Rectangle screenArea)
        {
            Vector2 pos = new Vector2(screenArea.X, screenArea.Y);

            if (screenArea.X < Min.X)
                pos.X = Min.X;
            else if (pos.X > Max.X - screenArea.Width)
                pos.X = Max.X - screenArea.Width;

            if (screenArea.Y < Min.Y)
                pos.Y = Min.Y;
            else if (pos.Y > Max.Y - screenArea.Height)
                pos.Y = Max.Y - screenArea.Height;

            return pos;
        }

        #endregion
    }
}