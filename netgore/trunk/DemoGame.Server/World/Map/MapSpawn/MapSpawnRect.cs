using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Server.DbObjs;
using log4net;
using Microsoft.Xna.Framework;
using NetGore;

namespace DemoGame.Server
{
    /// <summary>
    /// Describes a rectangle that describes the area in a Map where spawning will take place.
    /// </summary>
    public struct MapSpawnRect
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The height of the rectangle. If null, the rectangle will be stretched to fit the height of the Map.
        /// </summary>
        public ushort? Height;

        /// <summary>
        /// The width of the rectangle. If null, the rectangle will be stretched to fit the width of the Map.
        /// </summary>
        public ushort? Width;

        /// <summary>
        /// The X co-ordinate of the rectangle. If null, the rectangle will be located at the left side of the Map.
        /// </summary>
        public ushort? X;

        /// <summary>
        /// The Y co-ordinate of the rectangle. If null, the rectangle will be located at the top side of the Map.
        /// </summary>
        public ushort? Y;

        public MapSpawnRect(IMapSpawnTable mapSpawnTable)
            : this(mapSpawnTable.X, mapSpawnTable.Y, mapSpawnTable.Width, mapSpawnTable.Height)
        {
        }

        public MapSpawnRect(ushort? x, ushort? y, ushort? width, ushort? height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public bool Equals(MapSpawnRect other)
        {
            return other.X.Equals(X) && other.Y.Equals(Y) && other.Width.Equals(Width) && other.Height.Equals(Height);
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="obj">Another object to compare to. 
        ///                 </param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (obj.GetType() != typeof(MapSpawnRect))
                return false;
            return Equals((MapSpawnRect)obj);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = (X.HasValue ? X.Value.GetHashCode() : 0);
                result = (result * 397) ^ (Y.HasValue ? Y.Value.GetHashCode() : 0);
                result = (result * 397) ^ (Width.HasValue ? Width.Value.GetHashCode() : 0);
                result = (result * 397) ^ (Height.HasValue ? Height.Value.GetHashCode() : 0);
                return result;
            }
        }

        static string GetString(ushort? value)
        {
            const string nullStr = "?";
            if (!value.HasValue)
                return nullStr;
            return value.Value.ToString();
        }

        /// <summary>
        /// Gets the MapSpawnRect as a Rectangle.
        /// </summary>
        /// <param name="map">The IMap used to resolve any null values.</param>
        /// <returns>The MapSpawnRect as a Rectangle.</returns>
        public Rectangle ToRectangle(IMap map)
        {
            int x = X.HasValue ? (int)X.Value : 0;
            int y = X.HasValue ? (int)X.Value : 0;
            int width = Width.HasValue ? Width.Value : (int)Math.Round(map.Width) - x;
            int height = Height.HasValue ? Height.Value : (int)Math.Round(map.Height) - y;

            const string errmsgMoved =
                "A spawn rectangle was being drawn off of the map." +
                " Its previous {0} location was {1} and has been moved to {2}.";

            const string errmsgSize =
                "A spawn rectangle was to big on the {0} axis" + " and has been resized to fit into the map.";

            if (x > map.Width)
            {
                int t = x;
                x = (int)(map.Height - width);
                log.FatalFormat(errmsgMoved, "X", t, x);
                Debug.Fail(string.Format(errmsgMoved, "X", t, x));
            }

            if (x < 0)
            {
                log.FatalFormat(errmsgMoved, "X", x, 0);
                Debug.Fail(string.Format(errmsgMoved, "X", x, 0));
                x = 0;
            }

            if (y > map.Height)
            {
                int t = y;
                y = (int)(map.Height - height);
                log.FatalFormat(errmsgMoved, "Y", t, y);
                Debug.Fail(string.Format(errmsgMoved, "Y", t, y));
            }

            if (y < 0)
            {
                log.FatalFormat(errmsgMoved, "X", x, 0);
                Debug.Fail(string.Format(errmsgMoved, "X", x, 0));
                y = 0;
            }

            if ((x + width) > map.Width)
            {
                width = (int)(map.Width - x);
                log.FatalFormat(errmsgSize, "X");
                Debug.Fail(string.Format(errmsgSize, "X"));
            }

            if ((y + height) > map.Height)
            {
                height = (int)(map.Height - y);
                log.FatalFormat(errmsgSize, "Y");
                Debug.Fail(string.Format(errmsgSize, "Y"));
            }

            return new Rectangle(x, y, width, height);
        }

        public override string ToString()
        {
            return string.Format("({0},{1})x({2},{3})", GetString(X), GetString(Y), GetString(Width), GetString(Height));
        }

        public static bool operator ==(MapSpawnRect l, MapSpawnRect r)
        {
            return (l.X == r.X) && (l.Y == r.Y) && (l.Width == r.Width) && (l.Height == r.Height);
        }

        public static bool operator !=(MapSpawnRect l, MapSpawnRect r)
        {
            return !(l == r);
        }
    }
}