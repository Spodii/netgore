using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Xna.Framework;

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

        /// <summary>
        /// Transformation matrix to be used on the SpriteBatch.Begin call.
        /// </summary>
        Matrix _matrix = Matrix.Identity;

        /// <summary>
        /// Coordinate of the center of the top-left corner of the camera.
        /// </summary>
        Vector2 _min = Vector2.Zero;

        /// <summary>
        /// Rotation magnitude in radians.
        /// </summary>
        float _rotation = 0.0f;

        /// <summary>
        /// Scale percent (1 = 100% = normal).
        /// </summary>
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

        /// <summary>
        /// Update the transformation matrix.
        /// </summary>
        void UpdateMatrix()
        {
            // Force the camera to stay in the map
            if (KeepInMap && Map != null)
            {
                // Check the min values
                if (_min.X < 0)
                    _min.X = 0;
                if (_min.Y < 0)
                    _min.Y = 0;

                // Check the max values
                var max = Max;
                if (max.X > Map.Width)
                    _min.X = Map.Width - (Size.X / Scale);

                if (max.Y > Map.Height)
                    _min.Y = Map.Height - (Size.Y / Scale);
            }

            // Update the matrix
            Vector3 origin = new Vector3(_min, 0);
            _matrix = Matrix.CreateTranslation(-origin) * Matrix.CreateScale(_scale) * Matrix.CreateRotationZ(_rotation);
        }

        #region ICamera2D Members

        /// <summary>
        /// Gets the coordinate of the center of the <see cref="ICamera2D"/>'s view port.
        /// </summary>
        public Vector2 Center
        {
            get { return Min + Size / 2; }
        }

        /// <summary>
        /// Gets a <see cref="Rectangle"/> that describes the region of the world area visible by the camera.
        /// </summary>
        /// <returns>A <see cref="Rectangle"/> that describes the region of the world area visible by the camera.</returns>
        public Rectangle GetViewArea()
        {
            return new Rectangle((int)Min.X, (int)Min.Y, (int)Size.X, (int)Size.Y);
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
                UpdateMatrix();
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
                if (_map != null && KeepInMap)
                    UpdateMatrix();
            }
        }

        /// <summary>
        /// Gets a transformation matrix used to transform coordinates to respect the camera's settings.
        /// </summary>
        public Matrix Matrix
        {
            get { return _matrix; }
        }

        /// <summary>
        /// Gets the coordinates of the bottom-right corner of the camera's visible area.
        /// </summary>
        public Vector2 Max
        {
            get { return Min + Size / Scale; }
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
                UpdateMatrix();
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
                UpdateMatrix();
            }
        }

        /// <summary>
        /// Gets or sets the camera scale percent where 1 equals 100%, or no scaling.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is less than or equal to
        /// <see cref="float.Epsilon"/>.</exception>
        public float Scale
        {
            get { return _scale; }
            set
            {
                // Check that the value is different
                if (_scale == value)
                    return;

                if (value <= float.Epsilon)
                    throw new ArgumentOutOfRangeException("value");

                // Update the value and the matrix
                _scale = value;
                UpdateMatrix();
            }
        }

        /// <summary>
        /// Gets or sets the size of the camera's view area in pixels.
        /// </summary>
        public Vector2 Size
        {
            get { return _size; }
            set { _size = value; }
        }

        /// <summary>
        /// Centers the camera on an <see cref="Entity"/> so that the center of the <see cref="Entity"/> is at the center
        /// of the camera.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> to center on.</param>
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
        /// Moves the camera's position using the given <paramref name="offset"/>.
        /// </summary>
        /// <param name="offset">The amount and direction to move the camera.</param>
        public void Translate(Vector2 offset)
        {
            Min += offset;
        }

        /// <summary>
        /// Checks if a specified object is in view of the camera.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> to check.</param>
        /// <returns>True if in the view area; otherwise false.</returns>
        public bool InView(Entity entity)
        {
            if (entity == null)
            {
                const string errmsg = "entity is null.";
                Debug.Fail(errmsg);
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                return false;
            }

            return InView(entity.Position, entity.Size);
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
            var max = position + size;
            return (max.X > Min.X) && (max.Y > Min.Y) && (position.X < Max.X) && (position.Y < Max.Y);
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

        #endregion
    }
}