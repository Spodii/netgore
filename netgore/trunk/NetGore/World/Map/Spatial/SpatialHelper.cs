using System.Linq;
using SFML.Graphics;

namespace NetGore.World
{
    /// <summary>
    /// Helper methods for the <see cref="ISpatial"/> and spatial-related stuff.
    /// </summary>
    public static class SpatialHelper
    {
        /// <summary>
        /// Creates a Vector2 for the MTD based on the side.
        /// </summary>
        /// <param name="side">Side the collision occured on.</param>
        /// <param name="mtd">MTD value.</param>
        /// <returns>Vector2 for the MTD for the given <paramref name="side"/>.</returns>
        static Vector2 CreateMTDVector(BoxSide side, float mtd)
        {
            switch (side)
            {
                case BoxSide.Top: return new Vector2(0, mtd);
                case BoxSide.Bottom: return new Vector2(0, -mtd);
                case BoxSide.Right: return new Vector2(mtd, 0);
                case BoxSide.Left: return new Vector2(-mtd, 0);
                default: return Vector2.Zero;
            }
        }

        /// <summary>
        /// Finds the Minimal Translational Distance between two <see cref="ISpatial"/>s.
        /// </summary>
        /// <param name="source">Source (dynamic) <see cref="ISpatial"/> that will be the one moving.</param>
        /// <param name="target">Target (static) <see cref="ISpatial"/> that will not move.</param>
        /// <returns>The MTD for the <paramref name="source"/> to no longer intersect the <paramref name="target"/>.</returns>
        public static Vector2 MTD(ISpatial source, ISpatial target)
        {
            Vector2 srcMin = source.Position;
            Vector2 srcMax = source.Max;
            Vector2 tarMin = target.Position;
            Vector2 tarMax = target.Max;

            return MTD(srcMin, srcMax, tarMin, tarMax);
        }

        /// <summary>
        /// Finds the Minimal Translational Distance between two <see cref="ISpatial"/>s.
        /// </summary>
        /// <param name="srcMin">Top-left of the source (dynamic) <see cref="ISpatial"/> that will be the one moving.</param>
        /// <param name="srcMax">Bottom-right of the source (dynamic) <see cref="ISpatial"/> that will be the one moving.</param>
        /// <param name="tarMin">Top-left of the target (static) <see cref="ISpatial"/> that will not move.</param>
        /// <param name="tarMax">Bottom-right of the target (static) <see cref="ISpatial"/> that will not move.</param>
        /// <returns>The MTD for the source to no longer intersect the target.</returns>
        public static Vector2 MTD(Vector2 srcMin, Vector2 srcMax, Vector2 tarMin, Vector2 tarMax)
        {
            // Down
            float mtd = srcMax.Y - tarMin.Y;
            BoxSide side = BoxSide.Bottom;

            // Left
            float diff = srcMax.X - tarMin.X;
            if (diff < mtd)
            {
                mtd = diff;
                side = BoxSide.Left;
            }

            // Right
            diff = tarMax.X - srcMin.X;
            if (diff < mtd)
            {
                mtd = diff;
                side = BoxSide.Right;
            }

            // Up
            diff = tarMax.Y - srcMin.Y;
            if (diff < mtd)
            {
                mtd = diff;
                side = BoxSide.Top;
            }

            if (mtd < 0.0f)
                return Vector2.Zero;

            return CreateMTDVector(side, mtd + 1);
        }

        /// <summary>
        /// Creates a <see cref="Rectangle"/> describing the area of a <see cref="ISpatial"/>.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/> to get the <see cref="Rectangle"/> for.</param>
        /// <returns>A <see cref="Rectangle"/> describing the area of a <see cref="ISpatial"/>.</returns>
        public static Rectangle ToRectangle(ISpatial spatial)
        {
            Vector2 pos = spatial.Position;
            Vector2 size = spatial.Size;
            return new Rectangle(pos.X, pos.Y, size.X, size.Y);
        }
    }
}