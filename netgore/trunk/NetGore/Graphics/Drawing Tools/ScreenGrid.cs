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
    /// Creates a grid of uniform size that can be drawn to the screen.
    /// </summary>
    public class ScreenGrid : IPersistable
    {
        Color _color = new Color(255, 255, 255, 75);
        Vector2 _size = new Vector2(32);

        /// <summary>
        /// Gets or sets the color of the grid lines.
        /// </summary>
        [SyncValue]
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        /// <summary>
        /// Gets or sets the height of the grid in pixels. Must be greater than zero.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is less than
        /// or equal to zero.</exception>
        [Browsable(false)]
        public float Height
        {
            get { return Size.Y; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value", "Height must be greater than 0.");

                _size.Y = value;
            }
        }

        /// <summary>
        /// Gets or sets the size of the <see cref="ScreenGrid"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Either the X or Y on the <paramref name="value"/> is less than
        /// or equal to zero.</exception>
        [SyncValue]
        public Vector2 Size
        {
            get { return _size; }
            set
            {
                if (value.X <= 0 || value.Y <= 0)
                    throw new ArgumentOutOfRangeException("value", "Size's X and Y both must be greater than 0.");

                _size = value;
            }
        }

        /// <summary>
        /// Gets or sets the width of the grid in pixels. Must be greater than zero.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is less than
        /// or equal to zero.</exception>
        [Browsable(false)]
        public float Width
        {
            get { return Size.X; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value", "Width must be greater than 0.");

                _size.X = value;
            }
        }

        /// <summary>
        /// Aligns an object's position to the grid.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> to edit.</param>
        public void Align(Entity entity)
        {
            if (entity == null)
            {
                Debug.Fail("entity is null.");
                return;
            }

            var x = (float)(Math.Round(entity.Position.X / Width)) * Width;
            var y = (float)(Math.Round(entity.Position.Y / Height)) * Height;
            entity.Position = new Vector2(x, y);
        }

        /// <summary>
        /// Aligns a position to the grid, forcing rounding down.
        /// </summary>
        /// <param name="pos">Vector position to align to the grid.</param>
        /// <returns>Vector aligned to the grid.</returns>
        public Vector2 AlignDown(Vector2 pos)
        {
            var x = (float)(Math.Floor(pos.X / Width) * Width);
            var y = (float)(Math.Floor(pos.Y / Height) * Height);
            return new Vector2(x, y);
        }

        /// <summary>
        /// Draws the grid.
        /// </summary>
        /// <param name="sb">The <see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="camera">The <see cref="Camera2D"/> describing the view.</param>
        public void Draw(ISpriteBatch sb, ICamera2D camera)
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

            var p1 = new Vector2();
            var p2 = new Vector2();

            var min = camera.Min;
            var max = camera.Max;
            var size = camera.Size;
            min -= new Vector2(min.X % Size.X, min.Y % Size.Y);

            // Vertical lines
            p1.Y = (float)Math.Round(min.Y);
            p2.Y = (float)Math.Round(p1.Y + size.Y + 32);
            for (var x = min.X; x < max.X; x += Size.X)
            {
                p1.X = (float)Math.Round(x);
                p2.X = (float)Math.Round(x);
                RenderLine.Draw(sb, p1, p2, Color);
            }

            // Horizontal lines
            p1.X = (float)Math.Round(camera.Min.X);
            p2.X = (float)Math.Round(p1.X + size.X + 32);
            for (var y = min.Y; y < max.Y; y += Size.Y)
            {
                p1.Y = (float)Math.Round(y);
                p2.Y = (float)Math.Round(y);
                RenderLine.Draw(sb, p1, p2, Color);
            }
        }

        /// <summary>
        /// Snaps an <see cref="Entity"/>'s position to the grid. Intended for when moving an <see cref="Entity"/>.
        /// </summary>
        /// <param name="entity"><see cref="Entity"/> to snap to the grid.</param>
        public void SnapToGridPosition(Entity entity)
        {
            if (entity == null)
            {
                Debug.Fail("entity is null.");
                return;
            }

            var newPos = (entity.Position / Size).Round() * Size;

            entity.Position = newPos;
        }

        /// <summary>
        /// Snaps an <see cref="Entity"/>'s size to the grid. Intended for when resizing an <see cref="Entity"/>.
        /// </summary>
        /// <param name="entity"><see cref="Entity"/> to snap to the grid.</param>
        public void SnapToGridSize(Entity entity)
        {
            if (entity == null)
            {
                Debug.Fail("entity is null.");
                return;
            }

            var newSize = (entity.Size / Size).Round() * Size;

            if (newSize.X < Size.X)
                newSize.X = Size.X;
            if (newSize.Y < Size.Y)
                newSize.Y = Size.Y;

            entity.Size = newSize;
        }

        #region IPersistable Members

        /// <summary>
        /// Reads the state of the object from an <see cref="IValueReader"/>. Values should be read in the exact
        /// same order as they were written.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public void ReadState(IValueReader reader)
        {
            PersistableHelper.Read(this, reader);
        }

        /// <summary>
        /// Writes the state of the object to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        public void WriteState(IValueWriter writer)
        {
            PersistableHelper.Write(this, writer);
        }

        #endregion
    }
}