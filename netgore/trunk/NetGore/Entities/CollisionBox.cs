using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore
{
    /// <summary>
    /// A box defined by a min and max point that works as the base of all collision. Along with
    /// performing simple rectangular collision, the MTD can be used for collision response
    /// of multiple different types.
    /// </summary>
    public class CollisionBox
    {
        Vector2 _min;
        Vector2 _size;

        /// <summary>
        /// Gets the height of the CollisionBox
        /// </summary>
        public float Height
        {
            get { return _size.Y; }
        }

        /// <summary>
        /// Gets the maximum (bottom-right) point of the CollisionBox
        /// </summary>
        public Vector2 Max
        {
            get { return _min + _size; }
        }

        /// <summary>
        /// Gets the minimum (top-left) point of the CollisionBox
        /// </summary>
        public Vector2 Min
        {
            get { return _min; }
        }

        /// <summary>
        /// Gets the size of the CollisionBox
        /// </summary>
        public Vector2 Size
        {
            get { return _size; }
        }

        /// <summary>
        /// Gets the width of the CollisionBox
        /// </summary>
        public float Width
        {
            get { return _size.X; }
        }

        /// <summary>
        /// CollisionBox constructor
        /// </summary>
        /// <param name="position">Starting position</param>
        /// <param name="width">Width of the box</param>
        /// <param name="height">Height of the box</param>
        public CollisionBox(Vector2 position, float width, float height) : this(position, position + new Vector2(width, height))
        {
        }

        /// <summary>
        /// CollisionBox constructor
        /// </summary>
        /// <param name="width">Width of the box</param>
        /// <param name="height">Height of the box</param>
        public CollisionBox(float width, float height) : this(Vector2.Zero, new Vector2(width, height))
        {
        }

        /// <summary>
        /// CollisionBox constructor
        /// </summary>
        /// <param name="min">Position the box starts at</param>
        /// <param name="max">Position the box ends at</param>
        public CollisionBox(Vector2 min, Vector2 max)
        {
            // Make sure min is actually min while max is actually max
            if (min.X > max.X)
                Swap(ref min.X, ref max.X);
            if (min.Y > max.Y)
                Swap(ref min.Y, ref max.Y);

            _size = max - min;
            Teleport(min);
        }

        /// <summary>
        /// Creates a Vector2 for the MTD based on the side
        /// </summary>
        /// <param name="side">Side the collision occured on</param>
        /// <param name="mtd">MTD value</param>
        /// <returns>Vector2 representing the MTD</returns>
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
        /// Checks if the CollisionBox contains a point
        /// </summary>
        /// <param name="p">Point to check against</param>
        /// <returns>True if the CollisionBox contains point p, else false</returns>
        public bool HitTest(Vector2 p)
        {
            return (Min.X <= p.X && Max.X >= p.X && Min.Y <= p.Y && Max.Y >= p.Y);
        }

        /// <summary>
        /// Checks if two CollisionBoxes occupy any common space
        /// </summary>
        /// <param name="cb1">First CollisionBox to check</param>
        /// <param name="cb2">Second CollisionBox to check</param>
        /// <returns>True if occupy any common space, else false</returns>
        public static bool Intersect(CollisionBox cb1, CollisionBox cb2)
        {
            if (cb1.Max.X >= cb2.Min.X && cb1.Max.Y >= cb2.Min.Y && cb1.Min.X <= cb2.Max.X && cb1.Min.Y <= cb2.Max.Y)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks if a CollisionBox and Rectangle occupy any common space
        /// </summary>
        /// <param name="cb">CollisionBox to check</param>
        /// <param name="rect">Rectangle to check</param>
        /// <returns>True if occupy any common space, else false</returns>
        public static bool Intersect(CollisionBox cb, Rectangle rect)
        {
            if (cb.Max.X >= rect.X && cb.Max.Y >= rect.Y && cb.Min.X <= rect.Right && cb.Min.Y <= rect.Bottom)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks if two CollisionBoxes occupy any common space
        /// </summary>
        /// <param name="cb">CollisionBox to check agains</param>
        /// <returns>True if occupy any common space, else false</returns>
        public bool Intersect(CollisionBox cb)
        {
            return Intersect(this, cb);
        }

        /// <summary>
        /// Checks if a CollisionBox and Rectangle occupy any common space
        /// </summary>
        /// <param name="rect">Rectangle to check agains</param>
        /// <returns>True if occupy any common space, else false</returns>
        public bool Intersect(Rectangle rect)
        {
            return Intersect(this, rect);
        }

        /// <summary>
        /// Translates the collision box a defined amount from its current location
        /// </summary>
        /// <param name="distance">Translation amount</param>
        internal void Move(Vector2 distance)
        {
            _min += distance;
        }

        /// <summary>
        /// Finds the Minimal Translational Distance between two CollisionBoxes
        /// </summary>
        /// <param name="source">Source (dynamic) CollisionBox that will be the one moving</param>
        /// <param name="target">Target (static) CollisionBox that will not move</param>
        /// <param name="ct">Type of collision check to perform</param>
        /// <returns>The MTD for the source CollisionBox to no longer intersect the target CollisionBox</returns>
        public static Vector2 MTD(CollisionBox source, CollisionBox target, CollisionType ct)
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
                default:
                    return MTDFull(source, target);
            }
        }

        /// <summary>
        /// Finds the MTD for two Full CollisionBoxes
        /// </summary>
        /// <param name="source">Source (dynamic) CollisionBox that will be the one moving</param>
        /// <param name="target">Target (static) CollisionBox that will not move</param>
        /// <returns>The MTD for the source CollisionBox to no longer intersect the target CollisionBox</returns>
        static Vector2 MTDFull(CollisionBox source, CollisionBox target)
        {
            // Down
            float diff = source.Max.Y - target.Min.Y;
            if (diff < 0.0f)
                return Vector2.Zero;
            float mtd = diff;
            BoxSide side = BoxSide.Bottom;

            // Left
            diff = source.Max.X - target.Min.X;
            if (diff < 0.0f)
                return Vector2.Zero;
            if (diff < mtd)
            {
                mtd = diff;
                side = BoxSide.Left;
            }

            // Right
            diff = target.Max.X - source.Min.X;
            if (diff < 0.0f)
                return Vector2.Zero;
            if (diff < mtd)
            {
                mtd = diff;
                side = BoxSide.Right;
            }

            // Up
            diff = target.Max.Y - source.Min.Y;
            if (diff < 0.0f)
                return Vector2.Zero;
            if (diff < mtd)
            {
                mtd = diff;
                side = BoxSide.Top;
            }

            // Intersection occurred
            return CreateMTDVector(side, mtd);
        }

        /// <summary>
        /// Finds the MTD for a Full source CollisionBox and TriangleTopLeft target CollisionBox
        /// </summary>
        /// <param name="source">Source (dynamic) CollisionBox that will be the one moving</param>
        /// <param name="target">Target (static) CollisionBox that will not move</param>
        /// <returns>The MTD for the source CollisionBox to no longer intersect the target CollisionBox</returns>
        static Vector2 MTDTriangleTopLeft(CollisionBox source, CollisionBox target)
        {
            // Right
            float diff = target.Max.X - source.Min.X;
            if (diff < 0.0f)
                return Vector2.Zero;
            float mtd = diff;
            BoxSide side = BoxSide.Right;

            // Up
            diff = target.Max.Y - source.Min.Y;
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
                diff = source.Max.X - target.Min.X;
                if (diff < 0.0f)
                    return Vector2.Zero;
                if (diff < mtd)
                {
                    mtd = diff;
                    side = BoxSide.Left;
                }
            }

            // Down
            float h = source.Max.X - target.Min.X;
            if (h < 0)
                return Vector2.Zero;

            if (h >= target.Width)
                diff = source.Max.Y - target.Min.Y;
            else
                diff = source.Max.Y - (target.Max.Y - (target.Height * (h / target.Width)));

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
        /// Finds the MTD for a Full source CollisionBox and TriangleTopRight target CollisionBox
        /// </summary>
        /// <param name="source">Source (dynamic) CollisionBox that will be the one moving</param>
        /// <param name="target">Target (static) CollisionBox that will not move</param>
        /// <returns>The MTD for the source CollisionBox to no longer intersect the target CollisionBox</returns>
        static Vector2 MTDTriangleTopRight(CollisionBox source, CollisionBox target)
        {
            // Left
            float diff = source.Max.X - target.Min.X;
            if (diff < 0.0f)
                return Vector2.Zero;
            float mtd = diff;
            BoxSide side = BoxSide.Left;

            // Up
            diff = target.Max.Y - source.Min.Y;
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
                diff = target.Max.X - source.Min.X;
                if (diff < 0.0f)
                    return Vector2.Zero;
                if (diff < mtd)
                {
                    mtd = diff;
                    side = BoxSide.Right;
                }
            }

            // Down
            if (source.Min.X < target.Min.X)
                diff = source.Max.Y - target.Min.Y;
            else
            {
                float h = source.Min.X - target.Min.X;
                diff = source.Max.Y - (target.Min.Y + (target.Height * (h / target.Width)));
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

        /// <summary>
        /// Resizes the CollisionBox
        /// </summary>
        /// <param name="size">New size of the CollisionBox</param>
        internal void Resize(Vector2 size)
        {
            _size = size;
        }

        /// <summary>
        /// Swaps two floats
        /// </summary>
        /// <param name="a">First float</param>
        /// <param name="b">Second float</param>
        static void Swap(ref float a, ref float b)
        {
            float c = a;
            a = b;
            b = c;
        }

        /// <summary>
        /// Moves the collision box to a new location
        /// </summary>
        /// <param name="position">New position</param>
        internal void Teleport(Vector2 position)
        {
            _min = position;
        }

        /// <summary>
        /// Creates a rectangle that represents the position and size of the CollisionBox
        /// </summary>
        /// <returns>A rectangle that represents the position and size of the CollisionBox</returns>
        public Rectangle ToRectangle()
        {
            return new Rectangle((int)Min.X, (int)Min.Y, (int)Width, (int)Height);
        }
    }
}