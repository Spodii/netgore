using System.Linq;
using SFML.Graphics;

namespace NetGore
{
    /// <summary>
    /// Helper methods for the <see cref="Direction"/> enum.
    /// </summary>
    public static class DirectionHelper
    {
        /// <summary>
        /// Finds the <see cref="Direction"/> from a <see cref="Vector2"/>.
        /// </summary>
        /// <param name="v">The <see cref="Vector2"/> containing the relative direction, where
        /// (0,0) is the center.</param>
        /// <returns>The <see cref="Direction"/> for the <paramref name="v"/>, or null if
        /// <paramref name="v"/> is equal to <see cref="Vector2.Zero"/>.</returns>
        public static Direction? FromVector(Vector2 v)
        {
            if (v.Y < 0)
            {
                if (v.X < 0)
                    return Direction.NorthWest;
                else if (v.X == 0)
                    return Direction.North;
                else
                    return Direction.NorthEast;
            }
            else if (v.Y == 0)
            {
                if (v.X < 0)
                    return Direction.West;
                else if (v.X == 0)
                    return null;
                else
                    return Direction.East;
            }
            else
            {
                if (v.X < 0)
                    return Direction.SouthWest;
                else if (v.X == 0)
                    return Direction.South;
                else
                    return Direction.SouthEast;
            }
        }
    }
}