using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore
{
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
                case BoxSide.Top:
                    return new Vector2(0, mtd);
                case BoxSide.Bottom:
                    return new Vector2(0, -mtd);
                case BoxSide.Right:
                    return new Vector2(mtd, 0);
                case BoxSide.Left:
                    return new Vector2(-mtd, 0);
                default:
                    return Vector2.Zero;
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
            var srcMin = source.Position;
            var srcMax = source.Max;
            var tarMin = target.Position;
            var tarMax = target.Max;

            // Down
            float mtd = source.Max.Y - tarMin.Y;
            BoxSide side = BoxSide.Bottom;

            // Left
            var diff = srcMax.X - tarMin.X;
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

            return CreateMTDVector(side, mtd);
        }
    }
}