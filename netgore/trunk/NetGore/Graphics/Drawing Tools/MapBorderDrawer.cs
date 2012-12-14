using System;
using System.Linq;
using NetGore.World;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Draws an indicator showing where the map borders are.
    /// </summary>
    public class MapBorderDrawer
    {
        /// <summary>
        /// Creates a Rectangle from two points.
        /// </summary>
        /// <param name="min">Minimum points.</param>
        /// <param name="max">Maximum points.</param>
        /// <returns>Rectangle for the two points.</returns>
        static Rectangle CreateRect(Vector2 min, Vector2 max)
        {
            var size = max - min;
            return new Rectangle(min.X, min.Y, size.X, size.Y);
        }

        /// <summary>
        /// Draws the map borders.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="map">Map to draw the borders for.</param>
        /// <param name="camera">Camera used to view the map.</param>
        public virtual void Draw(ISpriteBatch sb, IMap map, ICamera2D camera)
        {
            if (sb == null || sb.IsDisposed)
                return;
            if (map == null)
                return;
            if (camera == null)
                return;

            // Left border and corners
            if (camera.Min.X < 0)
            {
                var min = camera.Min;
                var max = new Vector2(Math.Min(0, camera.Max.X), camera.Max.Y);
                DrawBorder(sb, min, max);
            }

            // Right border and corners
            if (camera.Max.X > map.Width)
            {
                var min = new Vector2(Math.Max(camera.Min.X, map.Width), camera.Min.Y);
                var max = camera.Max;
                DrawBorder(sb, min, max);
            }

            // Top border
            if (camera.Min.Y < 0)
            {
                var min = new Vector2(Math.Max(camera.Min.X, 0), camera.Min.Y);
                var max = new Vector2(Math.Min(camera.Max.X, map.Width), Math.Min(camera.Max.Y, 0));
                DrawBorder(sb, min, max);
            }

            // Bottom border
            if (camera.Max.Y > map.Height)
            {
                var min = new Vector2(Math.Max(camera.Min.X, 0), Math.Max(camera.Min.Y, map.Height));
                var max = new Vector2(Math.Min(camera.Max.X, map.Width), camera.Max.Y);
                DrawBorder(sb, min, max);
            }
        }

        /// <summary>
        /// Draws a border.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="min">Minimum point to draw.</param>
        /// <param name="max">Maximum point to draw.</param>
        protected virtual void DrawBorder(ISpriteBatch sb, Vector2 min, Vector2 max)
        {
            var drawColor = new Color(255, 0, 0, 175);
            RenderRectangle.Draw(sb, CreateRect(min, max), drawColor);
        }
    }
}