using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NetGore
{
    public static class SpatialHelper
    {
        /// <summary>
        /// Finds the Minimal Translational Distance between two <see cref="ISpatial"/>s.
        /// </summary>
        /// <param name="source">Source (dynamic) <see cref="ISpatial"/> that will be the one moving.</param>
        /// <param name="target">Target (static) <see cref="ISpatial"/> that will not move.</param>
        /// <param name="ct">The <see cref="CollisionType"/> that describes the actual collideable area inside
        /// the <see cref="target"/>. The <see cref="source"/> is assumed to be <see cref="CollisionType.Full"/>.</param>
        /// <returns>The MTD for the <paramref name="source"/> to no longer intersect the <paramref name="target"/>.</returns>
        public static Vector2 MTD(ISpatial source, ISpatial target, CollisionType ct)
        {
            // FUTURE: Would be nice if there was support for CollisionType on both the source and target
            switch (ct)
            {
                case CollisionType.Full:
                    return MTDFull(source, target);
                case CollisionType.TriangleTopLeft:
                    return MTDTriangleTopLeft(source, target);
                case CollisionType.TriangleTopRight:
                    return MTDTriangleTopRight(source, target);
                case CollisionType.None:
                    return Vector2.Zero;
                default:
                    throw new ArgumentOutOfRangeException("ct", string.Format("Unknown CollisionType `{0}`.", ct));
            }
        }

        /// <summary>
        /// Finds the MTD for two <see cref="ISpatial"/>s that use <see cref="CollisionType.Full"/>.
        /// </summary>
        /// <param name="source">Source (dynamic) <see cref="ISpatial"/> that will be the one moving.</param>
        /// <param name="target">Target (static) <see cref="ISpatial"/> that will not move.</param>
        /// <returns>The MTD for the <paramref name="source"/> to no longer intersect the <paramref name="target"/>.</returns>
        static Vector2 MTDFull(ISpatial source, ISpatial target)
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

        /// <summary>
        /// Finds the MTD for two <see cref="ISpatial"/>s where the <paramref name="target"/> uses
        /// <see cref="CollisionType.TriangleTopLeft"/>.
        /// </summary>
        /// <param name="source">Source (dynamic) <see cref="ISpatial"/> that will be the one moving.</param>
        /// <param name="target">Target (static) <see cref="ISpatial"/> that will not move.</param>
        /// <returns>The MTD for the <paramref name="source"/> to no longer intersect the <paramref name="target"/>.</returns>
        static Vector2 MTDTriangleTopLeft(ISpatial source, ISpatial target)
        {
            // Right
            float diff = target.Max.X - source.Position.X;
            if (diff < 0.0f)
                return Vector2.Zero;
            float mtd = diff;
            BoxSide side = BoxSide.Right;

            // Up
            diff = target.Max.Y - source.Position.Y;
            if (diff < 0.0f)
                return Vector2.Zero;
            if (diff < mtd)
            {
                mtd = diff;
                side = BoxSide.Top;
            }

            // Left
            if (Math.Abs(source.Max.Y - target.Max.Y) > 1)
            {
                diff = source.Max.X - target.Position.X;
                if (diff < 0.0f)
                    return Vector2.Zero;
                if (diff < mtd)
                {
                    mtd = diff;
                    side = BoxSide.Left;
                }
            }

            // Down
            float h = source.Max.X - target.Position.X;
            if (h < 0)
                return Vector2.Zero;

            if (h >= target.Size.X)
                diff = source.Max.Y - target.Position.Y;
            else
                diff = source.Max.Y - (target.Max.Y - (target.Size.Y * (h / target.Size.X)));

            if (diff < 0.0f)
                return Vector2.Zero;
            if (diff < mtd)
            {
                mtd = diff;
                side = BoxSide.Bottom;
            }

            // Intersection occurred
            return CreateMTDVector(side, mtd);
        }

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
        /// Finds the MTD for two <see cref="ISpatial"/>s where the <paramref name="target"/> uses
        /// <see cref="CollisionType.TriangleTopRight"/>.
        /// </summary>
        /// <param name="source">Source (dynamic) <see cref="ISpatial"/> that will be the one moving.</param>
        /// <param name="target">Target (static) <see cref="ISpatial"/> that will not move.</param>
        /// <returns>The MTD for the <paramref name="source"/> to no longer intersect the <paramref name="target"/>.</returns>
        static Vector2 MTDTriangleTopRight(ISpatial source, ISpatial target)
        {
            // Left
            float diff = source.Max.X - target.Position.X;
            if (diff < 0.0f)
                return Vector2.Zero;
            float mtd = diff;
            BoxSide side = BoxSide.Left;

            // Up
            diff = target.Max.Y - source.Position.Y;
            if (diff < 0.0f)
                return Vector2.Zero;
            if (diff < mtd)
            {
                mtd = diff;
                side = BoxSide.Top;
            }

            // Right
            if (Math.Abs(source.Max.Y - target.Max.Y) > 1)
            {
                diff = target.Max.X - source.Position.X;
                if (diff < 0.0f)
                    return Vector2.Zero;
                if (diff < mtd)
                {
                    mtd = diff;
                    side = BoxSide.Right;
                }
            }

            // Down
            if (source.Position.X < target.Position.X)
                diff = source.Max.Y - target.Position.Y;
            else
            {
                float h = source.Position.X - target.Position.X;
                diff = source.Max.Y - (target.Position.Y + (target.Size.Y * (h / target.Size.X)));
            }

            if (diff < 0.0f)
                return Vector2.Zero;
            if (diff < mtd)
            {
                mtd = diff;
                side = BoxSide.Bottom;
            }

            // Intersection occurred
            return CreateMTDVector(side, mtd);
        }

    }
}
