using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoGame.Server
{
    public struct MapSpawnRect
    {
        public ushort? X;
        public ushort? Y;
        public ushort? Width;
        public ushort? Height;

        public MapSpawnRect(ushort? x, ushort? y, ushort? width, ushort? height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public static bool operator ==(MapSpawnRect l, MapSpawnRect r)
        {
            return (l.X == r.X) && (l.Y == r.Y) && (l.Width == r.Width) && (l.Height == r.Height);
        }

        public static bool operator !=(MapSpawnRect l, MapSpawnRect r)
        {
            return !(l == r);
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
        ///                 </param><filterpriority>2</filterpriority>
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
        /// <filterpriority>2</filterpriority>
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
    }
}
