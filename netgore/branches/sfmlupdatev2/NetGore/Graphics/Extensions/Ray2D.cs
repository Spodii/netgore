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

        public Ray2D(Vector2 position, Vector2 direction, ISpatialCollection collection)
        {
            Position = position;
            Direction = direction;
            _mapSpatialCollection = collection;
        }


        private static void Swap<T>(ref T a, ref T b)
        {
            T c = a;
            a = b;
            b = c;
        }

        public bool Intersects<T>(out ISpatial spatial) where T: ISpatial
        {
            Vector2 nullVector;
            spatial = Cast<T>((int)Position.X, (int)Position.Y, (int)Direction.X, (int)Direction.Y, out nullVector);

            return (spatial != null);
        }

        public bool Intersects<T>(out Vector2 intersectingPoint) where T : ISpatial
        {
            var spatial = Cast<T>((int)Position.X, (int)Position.Y, (int)Direction.X, (int)Direction.Y, out intersectingPoint);

            return (spatial != null);
        }



        private ISpatial Cast<T>(int x0, int y0, int x1, int y1, out Vector2 finalPoint)
        {
            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);

            int sx = 0;
            int sy = 0;

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

            while (true)
            {

                finalPoint = new Vector2(x0, y0);

                    var res = _mapSpatialCollection.GetMany(finalPoint).OfType<T>();

                    if (res.Count() > 0)
                    {
                        return _mapSpatialCollection.Get(finalPoint);
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
                    y0 = y0 +sy;
                }
            }
            return null;
        }

       
    }


}

