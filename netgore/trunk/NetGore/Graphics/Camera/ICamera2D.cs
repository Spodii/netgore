using System;
using System.Linq;
using NetGore.World;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Interface for a 2D camera.
    /// </summary>
    public interface ICamera2D
    {
        /// <summary>
        /// Gets the coordinate of the center of the <see cref="ICamera2D"/>'s view port.
        /// </summary>
        Vector2 Center { get; }

        /// <summary>
        /// Gets or sets if the camera is forced to stay in view of the map. If true, the camera will never show anything
        /// outside of the range of the map. Only valid if <see cref="ICamera2D.Map"/> is not null.
        /// </summary>
        bool KeepInMap { get; set; }

        /// <summary>
        /// Gets or sets the map that this camera is viewing.
        /// </summary>
        IMap Map { get; set; }

        /// <summary>
        /// Gets the coordinates of the bottom-right corner of the camera's visible area.
        /// </summary>
        Vector2 Max { get; }

        /// <summary>
        /// Gets or sets the coordinates of the center of the top-left corner of the camera's visible area.
        /// </summary>
        Vector2 Min { get; set; }

        /// <summary>
        /// Gets or sets the camera's rotation magnitude in radians.
        /// </summary>
        float Rotation { get; set; }

        /// <summary>
        /// Gets or sets the camera scale percent where 1 equals 100%, or no scaling.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is less than or equal to
        /// <see cref="float.Epsilon"/>.</exception>
        float Scale { get; set; }

        /// <summary>
        /// Gets or sets the size of the camera's view area in pixels, taking the <see cref="ICamera2D.Scale"/> value
        /// into consideration.
        /// </summary>
        Vector2 Size { get; set; }

        /// <summary>
        /// Centers the camera on an <see cref="Entity"/> so that the center of the <see cref="Entity"/> is at the center
        /// of the camera.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> to center on.</param>
        void CenterOn(Entity entity);

        /// <summary>
        /// Centers the camera on a point so that the point is at the center of the camera.
        /// </summary>
        /// <param name="point">Point to center on.</param>
        void CenterOn(Vector2 point);

        /// <summary>
        /// Gets a <see cref="Rectangle"/> that describes the region of the world area visible by the camera.
        /// </summary>
        /// <returns>A <see cref="Rectangle"/> that describes the region of the world area visible by the camera.</returns>
        Rectangle GetViewArea();

        /// <summary>
        /// Checks if a specified object is in view of the camera.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/> to check if in view.</param>
        /// <returns>True if in the view area; otherwise false.</returns>
        bool InView(ISpatial spatial);

        /// <summary>
        /// Checks if a specified <see cref="Rectangle"/> is in view of the camera.
        /// </summary>
        /// <param name="rect">The <see cref="Rectangle"/> to check.</param>
        /// <returns>True if in the view area; otherwise false.</returns>
        bool InView(Rectangle rect);

        /// <summary>
        /// Checks if a specified object is in view of the camera.
        /// </summary>
        /// <param name="grh">The <see cref="Grh"/> to check.</param>
        /// <param name="position">The position of the <see cref="Grh"/>.</param>
        /// <returns>True if in the view area; otherwise false.</returns>
        bool InView(Grh grh, Vector2 position);

        /// <summary>
        /// Checks if a specified object is in view of the screen.
        /// </summary>
        /// <param name="position">Position of the object.</param>
        /// <param name="size">The size of the object.</param>
        /// <returns>True if in the view area, else false.</returns>
        bool InView(Vector2 position, Vector2 size);

        /// <summary>
        /// Translates a world position to a screen position.
        /// </summary>
        /// <param name="worldPosition">The world position.</param>
        /// <returns>The screen position for the given <see cref="worldPosition"/>.</returns>
        Vector2 ToScreen(Vector2 worldPosition);

        /// <summary>
        /// Translates a world position to a screen position.
        /// </summary>
        /// <param name="worldX">The world position X co-ordinate.</param>
        /// <param name="worldY">The world position Y co-ordinate.</param>
        /// <returns>The screen position for the given <see cref="worldX"/> and <see cref="worldY"/>.</returns>
        Vector2 ToScreen(float worldX, float worldY);

        /// <summary>
        /// Translates a screen position to world position.
        /// </summary>
        /// <param name="screenPosition">The screen position.</param>
        /// <returns>The world position for the given <see cref="screenPosition"/>.</returns>
        Vector2 ToWorld(Vector2 screenPosition);

        /// <summary>
        /// Translates a screen position to world position.
        /// </summary>
        /// <param name="screenX">The screen position X co-ordinate.</param>
        /// <param name="screenY">The screen position Y co-ordinate.</param>
        /// <returns>The world position for the given <see cref="screenX"/> and <see cref="screenY"/>.</returns>
        Vector2 ToWorld(float screenX, float screenY);

        /// <summary>
        /// Moves the camera's position using the given <paramref name="offset"/>.
        /// </summary>
        /// <param name="offset">The amount and direction to move the camera.</param>
        void Translate(Vector2 offset);

        /// <summary>
        /// Zooms the camera in and focuses on a specified point.
        /// </summary>
        /// <param name="origin">Point to zoom in on (the center of the view).</param>
        /// <param name="size">The original size of the camera view.</param>
        /// <param name="scale">Magnification scale in percent. Must be non-zero.</param>
        /// <exception cref="ArgumentException"><paramref name="scale"/> is equal to 0.</exception>
        void Zoom(Vector2 origin, Vector2 size, float scale);

        /// <summary>
        /// Finds the position for the given area to keep it in full view (or as much as possible) of the camera.
        /// </summary>
        /// <param name="screenArea">The area to keep in the screen (screen position).</param>
        /// <returns>The position for the screenArea to keep it in screen.</returns>
        Vector2 ClampScreenPosition(Rectangle screenArea);
    }
}