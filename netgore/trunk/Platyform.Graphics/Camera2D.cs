using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Xna.Framework;
using Platyform.Extensions;

namespace Platyform.Graphics
{
    /// <summary>
    /// Describes a camera for a 2D world
    /// </summary>
    public class Camera2D
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Transformation matrix to be used on the SpriteBatch.Begin call
        /// </summary>
        Matrix _matrix = Matrix.Identity;

        /// <summary>
        /// Coordinate of the center of the top-left corner of the camera
        /// </summary>
        Vector2 _min = Vector2.Zero;

        /// <summary>
        /// Rotation magnitude in radians
        /// </summary>
        float _rotation = 0.0f;

        /// <summary>
        /// Scale percent (1 = 100% = normal)
        /// </summary>
        float _scale = 1.0f;

        Vector2 _size;

        /// <summary>
        /// Gets the transformation matrix to be used on the SpriteBatch.Begin call
        /// </summary>
        public Matrix Matrix
        {
            get { return _matrix; }
        }

        /// <summary>
        /// Gets the coordinates of the bottom-right corner of the camera
        /// </summary>
        public Vector2 Max
        {
            get { return Min + Size; }
        }

        /// <summary>
        /// Gets or sets the coordinates of the center of the top-left corner of the camera
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
        /// Gets or sets the camera rotation magnitude in radians
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
        /// Gets or sets the camera scale percent (1 = 100% = normal)
        /// </summary>
        public float Scale
        {
            get { return _scale; }
            set
            {
                // Check that the value is different
                if (_scale == value)
                    return;

                // Update the value and the matrix
                _scale = value;
                UpdateMatrix();
            }
        }

        /// <summary>
        /// Gets or sets the size of the camera's view area
        /// </summary>
        public Vector2 Size
        {
            get { return _size; }
            set { _size = value; }
        }

        /// <summary>
        /// Gets or sets the top-left X coordinate of the camera position.
        /// </summary>
        public float X
        {
            get { return _min.X; }
            set
            {
                // Check that the value is different
                if (_min.X == value)
                    return;

                // Update the value and the matrix
                _min.X = value;
                UpdateMatrix();
            }
        }

        /// <summary>
        /// Gets or sets the top-left Y coordinate of the camera position.
        /// </summary>
        public float Y
        {
            get { return _min.Y; }
            set
            {
                // Check that the value is different
                if (_min.Y == value)
                    return;

                // Update the value and the matrix
                _min.Y = value;
                UpdateMatrix();
            }
        }

        /// <summary>
        /// Camera2D constructor
        /// </summary>
        /// <param name="screenSize">Size of the screen in pixels</param>
        public Camera2D(Vector2 screenSize)
        {
            _size = screenSize;
        }

        /// <summary>
        /// Centers the camera on an Entity so that the center of the Entity is at the center
        /// of the camera.
        /// </summary>
        /// <param name="entity">Entity to center on.</param>
        public void Center(Entity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Center(entity.CB.Min + (entity.CB.Size / 2));
        }

        /// <summary>
        /// Centers the camera on a point so that the point is at the center of the camera.
        /// </summary>
        /// <param name="point">Point to center on.</param>
        public void Center(Vector2 point)
        {
            Min = point - (Size / 2);
        }

        /// <summary>
        /// Checks if a specified object is in view of the camera
        /// </summary>
        /// <param name="entity">Entity to check</param>
        /// <returns>True if in the view area, else false</returns>
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

            return InView(entity.CB.Min, entity.CB.Max);
        }

        /// <summary>
        /// Checks if a specified object is in view of the camera
        /// </summary>
        /// <param name="grh">Grh to check</param>
        /// <param name="position">Position to draw the Grh at</param>
        /// <returns>True if in the view area, else false</returns>
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

            return InView(position, position + new Vector2(grh.Width, grh.Height));
        }

        /// <summary>
        /// Checks if a specified object is in view of the screen
        /// </summary>
        /// <param name="min">Minimum (top-left) vector</param>
        /// <param name="max">Maximum (bottom-right) vector</param>
        /// <returns>True if in the view area, else false</returns>
        public bool InView(Vector2 min, Vector2 max)
        {
            return (max.X > Min.X && max.Y > Min.Y && min.X < Min.X + _size.X && min.Y < Min.Y + _size.Y);
        }

        /// <summary>
        /// Checks if a specified object is in view of the screen
        /// </summary>
        /// <param name="x">X coordinate of the object</param>
        /// <param name="y">Y coordinate of the object</param>
        /// <param name="width">Width of the object</param>
        /// <param name="height">Height of the object</param>
        /// <returns>True if in the view area, else false</returns>
        public bool InView(float x, float y, float width, float height)
        {
            return (x + width > Min.X && y + height > Min.Y && x < Min.X + _size.X && y < Min.Y + _size.Y);
        }

        /// <summary>
        /// Translates a world position to a screen position
        /// </summary>
        /// <param name="p"></param>
        /// <returns>Screen point for the given world point</returns>
        public Vector2 ToScreen(Vector2 p)
        {
            return (p - Min) * Scale;
        }

        /// <summary>
        /// Translates a world position to a screen position
        /// </summary>
        /// <param name="x">World point x</param>
        /// <param name="y">World point x</param>
        /// <returns>Screen point for the given world point</returns>
        public Vector2 ToScreen(float x, float y)
        {
            return ToScreen(new Vector2(x, y));
        }

        /// <summary>
        /// Translates a screen position to world position
        /// </summary>
        /// <param name="p">Screen point</param>
        /// <returns>World point for the given screen point</returns>
        public Vector2 ToWorld(Vector2 p)
        {
            return (p / Scale) + Min;
        }

        /// <summary>
        /// Translates a screen position to world position
        /// </summary>
        /// <param name="x">Screen point x</param>
        /// <param name="y">Screen point x</param>
        /// <returns>World point for the given screen point</returns>
        public Vector2 ToWorld(float x, float y)
        {
            return ToWorld(new Vector2(x, y));
        }

        /// <summary>
        /// Update the transformation matrix
        /// </summary>
        void UpdateMatrix()
        {
            Vector3 origin = new Vector3(_min, 0);
            _matrix = Matrix.CreateTranslation(-origin) * Matrix.CreateScale(_scale) * Matrix.CreateRotationZ(_rotation);
        }

        /// <summary>
        /// Zooms the camera in and focuses on a specified point
        /// </summary>
        /// <param name="origin">Point to zoom in on</param>
        /// <param name="size">Original size of the camera view</param>
        /// <param name="scale">Magnification scale in percent</param>
        public void Zoom(Vector2 origin, Vector2 size, float scale)
        {
            Min = origin - Vector2.Divide(size, 2f * scale);
            Scale = scale;
        }
    }
}