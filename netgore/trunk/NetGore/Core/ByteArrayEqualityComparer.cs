using System.Collections.Generic;
using System.Linq;

namespace NetGore
{
    /// <summary>
    /// <see cref="IEqualityComparer{T}"/> for comparing the contents of a byte array.
    /// </summary>
    public class ByteArrayEqualityComparer : IEqualityComparer<byte[]>
    {
        static readonly ByteArrayEqualityComparer _instance;

        /// <summary>
        /// Initializes the <see cref="ByteArrayEqualityComparer"/> class.
        /// </summary>
        static ByteArrayEqualityComparer()
        {
            _instance = new ByteArrayEqualityComparer();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ByteArrayEqualityComparer"/> class.
        /// </summary>
        ByteArrayEqualityComparer()
        {
        }

        /// <summary>
        /// Gets the <see cref="ByteArrayEqualityComparer"/> instance.
        /// </summary>
        public static IEqualityComparer<byte[]> Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        public static unsafe bool AreEqual(byte[] x, byte[] y)
        {
            if (x == y)
                return true;

            if (x == null || y == null || x.Length != y.Length)
                return false;

            /*
            // Simple (slower) version:
            for (var i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i])
                    return false;
            }
            */

            // Do comparison using multiple bytes at a time to increase performance
            fixed (byte* px = x)
            {
                fixed (byte* py = y)
                {
                    byte* x1 = px;
                    byte* x2 = py;
                    int len = x.Length;

                    // Compare 64-bit
                    for (int i = 0; i < len / 8; i++, x1 += 8, x2 += 8)
                    {
                        if (*((long*)x1) != *((long*)x2)) return false;
                    }

                    // Compare 32-bit
                    if ((len & 4) != 0) 
                    { 
                        if (*((int*)x1) != *((int*)x2)) 
                            return false; 
                        x1 += 4; 
                        x2 += 4; 
                    }

                    // Compare 16-bit
                    if ((len & 2) != 0) 
                    { 
                        if (*((short*)x1) != *((short*)x2)) 
                            return false; 
                        x1 += 2; 
                        x2 += 2; 
                    }

                    // Compare 8-bit
                    if ((len & 1) != 0)
                    {
                        if (*((byte*)x1) != *((byte*)x2))
                            return false;
                    }

                    return true;
                }
            }
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> for which a hash code is to be returned.</param>
        /// <returns>A hash code for the specified object.</returns>
        /// <exception cref="T:System.ArgumentNullException">The type of <paramref name="obj"/> is a reference type and
        /// <paramref name="obj"/> is null.</exception>
        public static int GetHashCode(byte[] obj)
        {
            // FNV-style hash
            // http://bretm.home.comcast.net/~bretm/hash/6.html
            unchecked
            {
                const int p = 16777619;
                var hash = (int)2166136261;

                for (var i = 0; i < obj.Length; i++)
                {
                    hash = (hash ^ obj[i]) * p;
                }

                hash += hash << 13;
                hash ^= hash >> 7;
                hash += hash << 3;
                hash ^= hash >> 17;
                hash += hash << 5;
                return hash;
            }
        }

        #region IEqualityComparer<byte[]> Members

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        bool IEqualityComparer<byte[]>.Equals(byte[] x, byte[] y)
        {
            return AreEqual(x, y);
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> for which a hash code is to be returned.</param>
        /// <returns>A hash code for the specified object.</returns>
        /// <exception cref="T:System.ArgumentNullException">The type of <paramref name="obj"/> is a reference type and
        /// <paramref name="obj"/> is null.</exception>
        int IEqualityComparer<byte[]>.GetHashCode(byte[] obj)
        {
            return GetHashCode(obj);
        }

        #endregion
    }
}