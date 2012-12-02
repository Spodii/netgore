using SFML.Graphics;

namespace NetGore
{
    public static class PointExtensions
    {
        /// <summary>
        /// Converts a Point to a Vector2.
        /// </summary>
        public static Vector2 ToVector2(this Point point)
        {
            return new Vector2(point.X, point.Y);
        }
    }
}