using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.DbObjs;
using log4net;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame.Server
{
    /// <summary>
    /// Describes a rectangle that describes the area in a Map where spawning will take place.
    /// </summary>
    [TypeConverter(typeof(MapSpawnRectEditor))]
    public struct MapSpawnRect
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        ushort? _height;
        ushort? _width;
        ushort? _x;
        ushort? _y;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapSpawnRect"/> struct.
        /// </summary>
        /// <param name="mapSpawnTable">The map spawn table.</param>
        public MapSpawnRect(IMapSpawnTable mapSpawnTable)
            : this(mapSpawnTable.X, mapSpawnTable.Y, mapSpawnTable.Width, mapSpawnTable.Height)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapSpawnRect"/> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public MapSpawnRect(ushort? x, ushort? y, ushort? width, ushort? height)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }

        /// <summary>
        /// Gets or sets the height of the rectangle. If null, the rectangle will be stretched to fit the height of the Map.
        /// </summary>
        [DisplayName("Height")]
        [Browsable(true)]
        [DefaultValue(null)]
        [Description("The height of the spawn rectangle. If null, the rectangle will be stretched to fit the height of the Map.")]
        public ushort? Height
        {
            get { return _height; }
            set { _height = value; }
        }

        /// <summary>
        /// Gets or sets the width of the rectangle. If null, the rectangle will be stretched to fit the width of the Map.
        /// </summary>
        [DisplayName("Width")]
        [Browsable(true)]
        [DefaultValue(null)]
        [Description("The width of the spawn rectangle. If null, the rectangle will be stretched to fit the height of the Map.")]
        public ushort? Width
        {
            get { return _width; }
            set { _width = value; }
        }

        /// <summary>
        /// Gets or sets the X co-ordinate of the rectangle. If null, the rectangle will be located at the left side of the Map.
        /// </summary>
        [DisplayName("X")]
        [Browsable(true)]
        [DefaultValue(null)]
        [Description(
            "The X-coordinate of the spawn rectangle. If null, the rectangle will be located at the left side of the Map.")]
        public ushort? X
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// Gets or sets the Y co-ordinate of the rectangle. If null, the rectangle will be located at the top side of the Map.
        /// </summary>
        [DisplayName("Y")]
        [Browsable(true)]
        [DefaultValue(null)]
        [Description("The Y-coordinate of the spawn rectangle. If null, the rectangle will be located at the top side of the Map."
            )]
        public ushort? Y
        {
            get { return _y; }
            set { _y = value; }
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
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
        /// <param name="obj">Another object to compare to.</param>
        public override bool Equals(object obj)
        {
            return obj is MapSpawnRect && this == (MapSpawnRect)obj;
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
                var result = (X.HasValue ? X.Value.GetHashCode() : 0);
                result = (result * 397) ^ (Y.HasValue ? Y.Value.GetHashCode() : 0);
                result = (result * 397) ^ (Width.HasValue ? Width.Value.GetHashCode() : 0);
                result = (result * 397) ^ (Height.HasValue ? Height.Value.GetHashCode() : 0);
                return result;
            }
        }

        /// <summary>
        /// Gets the string to use for a nullable ushort.
        /// </summary>
        /// <param name="value">The value to get the string for.</param>
        /// <returns>The string to use for the <paramref name="value"/>.</returns>
        static string GetString(ushort? value)
        {
            const string nullStr = "?";
            if (!value.HasValue)
                return nullStr;
            return value.Value.ToString();
        }

        /// <summary>
        /// Gets the <see cref="MapSpawnRect"/> as a <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="map">The <see cref="IMap"/> used to resolve any null values.</param>
        /// <returns>The <see cref="MapSpawnRect"/> as a <see cref="Rectangle"/>.</returns>
        public Rectangle ToRectangle(IMap map)
        {
            int x = X.HasValue ? (int)X.Value : 0;
            int y = Y.HasValue ? (int)Y.Value : 0;
            int width = Width.HasValue ? (int)Width.Value : (int)Math.Round(map.Width) - x;
            int height = Height.HasValue ? (int)Height.Value : (int)Math.Round(map.Height) - y;

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

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("({0},{1})x({2},{3})", GetString(X), GetString(Y), GetString(Width), GetString(Height));
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="l">The first argument.</param>
        /// <param name="r">The second argument.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(MapSpawnRect l, MapSpawnRect r)
        {
            return (l.X == r.X) && (l.Y == r.Y) && (l.Width == r.Width) && (l.Height == r.Height);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="l">The first argument.</param>
        /// <param name="r">The second argument.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(MapSpawnRect l, MapSpawnRect r)
        {
            return !(l == r);
        }
    }
}