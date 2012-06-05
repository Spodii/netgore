using System;
using System.Linq;
using SFML.Graphics;

namespace NetGore.World
{
    /// <summary>
    /// Helper methods for the <see cref="Alignment"/> enum.
    /// </summary>
    public static class AlignmentHelper
    {
        /// <summary>
        /// Finds the <see cref="Vector2"/> that represents the position to align an object
        /// to another object based on the given <see cref="Alignment"/>. The object being aligned
        /// to is assumed to be at {0,0).
        /// </summary>
        /// <param name="alignment">The <see cref="Alignment"/> describing how to align the target
        /// to the source.</param>
        /// <param name="sourceSize">The object that is being aligned to the target.</param>
        /// <param name="targetSize">The size of the object being aligned to.</param>
        /// <returns>The <see cref="Vector2"/> that represents the position to align an object
        /// to another object based on the given <paramref name="alignment"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="alignment"/> is not a defined value of the <see cref="Alignment"/>
        /// enum.</exception>
        public static Vector2 FindOffset(Alignment alignment, Vector2 sourceSize, Vector2 targetSize)
        {
            switch (alignment)
            {
                case Alignment.TopLeft:
                    return new Vector2(0, 0);

                case Alignment.TopRight:
                    return new Vector2(targetSize.X - sourceSize.X, 0);

                case Alignment.BottomLeft:
                    return new Vector2(0, targetSize.Y - sourceSize.Y);

                case Alignment.BottomRight:
                    return targetSize - sourceSize;

                case Alignment.Top:
                    return new Vector2(targetSize.X / 2f - sourceSize.X / 2f, 0);

                case Alignment.Bottom:
                    return new Vector2(targetSize.X / 2f - sourceSize.X / 2f, targetSize.Y - sourceSize.Y);

                case Alignment.Left:
                    return new Vector2(0, targetSize.Y / 2f - sourceSize.Y / 2f);

                case Alignment.Right:
                    return new Vector2(targetSize.X - sourceSize.X, targetSize.Y / 2f - sourceSize.Y / 2f);

                case Alignment.Center:
                    return targetSize / 2f - sourceSize / 2f;

                default:
                    throw new ArgumentException("Unknown alignment value specified", "alignment");
            }
        }
    }
}