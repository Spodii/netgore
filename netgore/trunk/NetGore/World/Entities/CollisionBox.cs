using System;
using System.Linq;
using Microsoft.Xna.Framework;
using NetGore.Extensions;

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
        /// Initializes a new instance of the <see cref="CollisionBox"/> class.
        /// </summary>
        /// <param name="source">The <see cref="CollisionBox"/> to copy the values from.</param>
        public CollisionBox(CollisionBox source) : this(source.Min, source.Max)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollisionBox"/> class.
        /// </summary>
        /// <param name="position">Starting position</param>
        /// <param name="width">Width of the box</param>
        /// <param name="height">Height of the box</param>
        public CollisionBox(Vector2 position, float width, float height) : this(position, position + new Vector2(width, height))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollisionBox"/> class.
        /// </summary>
        /// <param name="width">Width of the box</param>
        /// <param name="height">Height of the box</param>
        public CollisionBox(float width, float height) : this(Vector2.Zero, new Vector2(width, height))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollisionBox"/> class.
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
        /// Translates the collision box a defined amount from its current location
        /// </summary>
        /// <param name="distance">Translation amount</param>
        public void Move(Vector2 distance)
        {
            // TODO: !! Do not have public
            _min += distance;
        }

        /// <summary>
        /// Resizes the CollisionBox
        /// </summary>
        /// <param name="size">New size of the CollisionBox</param>
        public void Resize(Vector2 size)
        {
            // TODO: !! Do not have public
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
        public void Teleport(Vector2 position)
        {
            // TODO: !! Do not have public
            _min = position;
        }

        /// <summary>
        /// Creates a rectangle that represents the position and size of the CollisionBox
        /// </summary>
        /// <returns>A rectangle that represents the position and size of the CollisionBox</returns>
        public Rectangle ToRectangle()
        {
            return new Rectangle((int)Min.X, (int)Min.Y, (int)Size.X, (int)Size.Y);
        }
    }
}