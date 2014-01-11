using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using NetGore.World;
using SFML.Graphics;

namespace NetGore.Graphics.Extensions
{

    // TODO: Most of the implementation/documentation of this type is Pending.

    /// <summary>
    /// Defines a ray that works on the 2D plane and can be used to query <see cref="ISpatial"/>'s for intersection.
    /// </summary>
    public struct Ray2D
    {
        public Vector2 Position;
        public Vector2 Direction;

        readonly ISpatialCollection _mapSpatialCollection;
        readonly ISpatial _owner;

        public Ray2D(ISpatial character, Vector2 position, Vector2 direction, ISpatialCollection collection)
        {
            Position = position;
            Direction = direction;
            _mapSpatialCollection = collection;
            _owner = character;
        }

        private static void Swap<T>(ref T a, ref T b)
        {
            T c = a;
            a = b;
            b = c;
        }

        public bool Intersects<T>(out List<ISpatial> spatial) where T : ISpatial
        {
            Vector2 nullVector;
            spatial = Cast<T>((int)Position.X, (int)Position.Y, (int)Direction.X, (int)Direction.Y, false, out nullVector);

            return (spatial != null);
        }

        public bool Intersects<T>(out Vector2 intersectingPoint) where T : ISpatial
        {
            var spatial = Cast<T>((int)Position.X, (int)Position.Y, (int)Direction.X, (int)Direction.Y, false, out intersectingPoint);

            return (spatial != null);
        }

        public bool IntersectsMany<T>(out List<ISpatial> spatial) where T : ISpatial
        {
            Vector2 nullVector;
            spatial = Cast<T>((int)Position.X, (int)Position.Y, (int)Direction.X, (int)Direction.Y, true, out nullVector);

            return (spatial != null);
        }

        public bool IntersectsMany<T>(out Vector2 intersectingPoint) where T : ISpatial
        {
            var spatial = Cast<T>((int)Position.X, (int)Position.Y, (int)Direction.X, (int)Direction.Y, true, out intersectingPoint);

            return (spatial != null);
        }

        private List<ISpatial> Cast<T>(int x0, int y0, int x1, int y1, bool needMany, out Vector2 finalPoint)
        {
            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);

            int sx;
            int sy;

            if (x0 < x1)
                sx = 1;
            else
                sx = -1;

            if (y0 < y1)
                sy = 1;
            else
                sy = -1;

            int err = dx - dy;
            int e2;
            var temp = new List<ISpatial>();

            while (true)
            {
                finalPoint = new Vector2(x0, y0);

                var res = _mapSpatialCollection.GetMany(finalPoint).OfType<T>();

                foreach (ISpatial s in res)
                {
                    // Don't add the same ISpatial twice
                    if (!temp.Contains(s))
                    {
                        // Don't add ourselves
                        if (s != _owner)
                            temp.Add(s);

                        // Only return the first if required
                        if (!temp.IsEmpty() && !needMany)
                            return temp;
                    }
                }

                if ((x0 == x1) && (y0 == y1))
                    break;

                e2 = err << 1;

                if (e2 > -dy)
                {
                    err = err - dy;
                    x0 = x0 + sx;
                }

                if (e2 < dx)
                {
                    err = err + dx;
                    y0 = y0 + sy;
                }
            }

            if (!temp.IsEmpty())
                return temp;

            return null;
        }
    }
}