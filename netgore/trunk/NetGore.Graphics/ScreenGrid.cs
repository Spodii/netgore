using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Draws a grid to the screen
    /// </summary>
    public class ScreenGrid
    {
        /// <summary>
        /// Color of the lines
        /// </summary>
        Color _color = new Color(255, 255, 255, 75);

        /// <summary>
        /// Gap between lines on the y-axis
        /// </summary>
        float _height = 32;

        /// <summary>
        /// Rectangle for drawing horizontal lines
        /// </summary>
        Rectangle _hLine;

        Vector2 _screenSize;

        /// <summary>
        /// Rectangle for drawing vertical lines
        /// </summary>
        Rectangle _vLine;

        /// <summary>
        /// Gap between lines on the x-axis
        /// </summary>
        float _width = 32;

        /// <summary>
        /// Gets or sets the color of the grid lines
        /// </summary>
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        /// <summary>
        /// Gets or sets the height of the grid in pixels
        /// </summary>
        public float Height
        {
            get { return _height; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value", "Width must be greater than 0.");
                _height = value;
            }
        }

        /// <summary>
        /// Horizontal line for the grid
        /// </summary>
        public Rectangle HLine
        {
            get { return _hLine; }
        }

        /// <summary>
        /// Gets or sets the size of the screen
        /// </summary>
        public Vector2 ScreenSize
        {
            get { return _screenSize; }
            set
            {
                if (_screenSize == value)
                    return;

                _screenSize = value;
                _vLine = new Rectangle(0, 0, 1, (int)_screenSize.Y);
                _hLine = new Rectangle(0, 0, (int)_screenSize.X, 1);
            }
        }

        /// <summary>
        /// Vertical line for the grid
        /// </summary>
        public Rectangle VLine
        {
            get { return _vLine; }
        }

        /// <summary>
        /// Gets or sets the width of the grid in pixels
        /// </summary>
        public float Width
        {
            get { return _width; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value", "Width must be greater than 0.");
                _width = value;
            }
        }

        /// <summary>
        /// ScreenGrid constructor
        /// </summary>
        /// <param name="screenSize">Size of the screen in pixels</param>
        public ScreenGrid(Vector2 screenSize)
        {
            _screenSize = screenSize;
        }

        /// <summary>
        /// Aligns an object's position to the grid
        /// </summary>
        /// <param name="entity">Entity to edit</param>
        public void Align<T>(T entity) where T : Entity
        {
            if (entity == null)
            {
                Debug.Fail("entity is null.");
                return;
            }

            float x = (float)(Math.Round(entity.Position.X / Width)) * Width;
            float y = (float)(Math.Round(entity.Position.Y / Height)) * Height;
            entity.Teleport(new Vector2(x, y));
        }

        /// <summary>
        /// Aligns a vector's position to the grid, forcing rounding down
        /// </summary>
        /// <param name="pos">Vector position to align to the grid</param>
        /// <returns>Vector aligned to the grid</returns>
        public Vector2 AlignDown(Vector2 pos)
        {
            return new Vector2((float)(Math.Floor(pos.X / Width) * Width), (float)(Math.Floor(pos.Y / Height) * Height));
        }

        /// <summary>
        /// Draws the grid to the SpriteBatch
        /// </summary>
        /// <param name="sb">SpriteBatch object to use (Begin and End must be handled externally)</param>
        /// <param name="camera">Camera to use</param>
        public void Draw(SpriteBatch sb, Camera2D camera)
        {
            if (sb == null)
            {
                Debug.Fail("sb is null.");
                return;
            }
            if (sb.IsDisposed)
            {
                Debug.Fail("sb is disposed.");
                return;
            }
            if (camera == null)
            {
                Debug.Fail("camera is null.");
                return;
            }

            Vector2 p1 = new Vector2();
            Vector2 p2 = new Vector2();

            Vector2 min = camera.Min;
            min.X -= min.X % _width;
            min.Y -= min.Y % _height;

            Vector2 max = camera.Min + _screenSize;

            // Vertical lines
            p1.Y = camera.Min.Y;
            p2.Y = p1.Y + _screenSize.Y;
            for (float x = min.X; x < max.X; x += _width)
            {
                p1.X = x;
                p2.X = x;
                XNALine.Draw(sb, p1, p2, Color);
            }

            // Horizontal lines
            p1.X = camera.Min.X;
            p2.X = p1.X + _screenSize.X;
            for (float y = min.Y; y < max.Y; y += _height)
            {
                p1.Y = y;
                p2.Y = y;
                XNALine.Draw(sb, p1, p2, Color);
            }
        }

        /// <summary>
        /// Snaps a entity's position to the grid
        /// </summary>
        /// <param name="entity">Entity to edit</param>
        public void SnapToGridPosition(Entity entity)
        {
            if (entity == null)
            {
                Debug.Fail("entity is null.");
                return;
            }

            Vector2 newPos;
            newPos.X = (float)(Math.Round(entity.CB.Min.X / Width) * Width);
            newPos.Y = (float)(Math.Round(entity.CB.Min.Y / Height) * Height);

            // TODO: map.SafeTeleportEntity()
            entity.Teleport(newPos);
        }

        /// <summary>
        /// Snaps a entity's size to the grid
        /// </summary>
        /// <param name="entity">Entity to edit</param>
        public void SnapToGridSize(Entity entity)
        {
            if (entity == null)
            {
                Debug.Fail("entity is null.");
                return;
            }

            Vector2 newSize;
            newSize.X = (float)(Math.Round(entity.CB.Width / Width) * Width);
            newSize.Y = (float)(Math.Round(entity.CB.Height / Height) * Height);

            if (newSize.X < Width)
                newSize.X = Width;
            if (newSize.Y < Height)
                newSize.Y = Height;

            // TODO: map.SafeResizeEntity()
            entity.Resize(newSize);
        }
    }
}