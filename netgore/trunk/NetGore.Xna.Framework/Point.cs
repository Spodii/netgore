using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Globalization;
using NetGore.Xna.Framework.Design;

namespace NetGore.Xna.Framework
{
    [Serializable, StructLayout(LayoutKind.Sequential), TypeConverter(typeof(PointConverter))]
    public struct Point : IEquatable<Point>
    {
        public int X;
        public int Y;
        private static Point _zero;
        public static Point Zero
        {
            get
            {
                return _zero;
            }
        }
        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public bool Equals(Point other)
        {
            return ((this.X == other.X) && (this.Y == other.Y));
        }

        public override bool Equals(object obj)
        {
            bool flag = false;
            if (obj is Point)
            {
                flag = this.Equals((Point)obj);
            }
            return flag;
        }

        public override int GetHashCode()
        {
            return (this.X.GetHashCode() + this.Y.GetHashCode());
        }

        public override string ToString()
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            return string.Format(currentCulture, "{{X:{0} Y:{1}}}", new object[] { this.X.ToString(currentCulture), this.Y.ToString(currentCulture) });
        }

        public static bool operator ==(Point a, Point b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Point a, Point b)
        {
            if (a.X == b.X)
            {
                return (a.Y != b.Y);
            }
            return true;
        }

        static Point()
        {
            _zero = new Point();
        }
    }


}
