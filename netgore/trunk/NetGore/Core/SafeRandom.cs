using System;
using System.Linq;

namespace NetGore
{
    /// <summary>
    /// A replacement for <see cref="System.Random"/> for when generating random numbers across
    /// multiple threads with the same random number generator object instance. Unlike <see cref="System.Random"/>,
    /// this will not become indefinitely corrupt when called from multiple threads. <see cref="System.Random"/>,
    /// when called from multiple threads, can generate a state corruption of the internal seed array, eventually
    /// resulting in the seed values becoming 0. Once the right two seed values become 0, the rest of the seeds
    /// will end up becoming 0, resulting in the generator becoming forever stuck in returning 0 for the internal
    /// random number generation.
    /// 
    /// The only times you should use <see cref="System.Random"/> instead of <see cref="SafeRandom"/> is when you
    /// need a more reliable set (at least a few hundred numbers) of random values and do not need thread safety.
    /// </summary>
    public class SafeRandom : Random
    {
        /* Implementation based off of George Marsaglia's Xorshift RNG
         *      http://www.jstatsoft.org/v08/i14/xorshift.pdf
         * C# port based off of Colin Green's FastRandom class
         *      http://www.codeproject.com/KB/cs/fastrandom.aspx
         */

        const uint _cW = 273326509;
        const uint _cY = 842502087;
        const uint _cZ = 3579807591;
        const double _realUnitInt = 1.0 / (int.MaxValue + 1.0);
        const double _realUnitUInt = 1.0 / (uint.MaxValue + 1.0);

        uint _bitBuffer;
        uint _bitMask = 1;
        uint _w;
        uint _x;
        uint _y;
        uint _z;

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeRandom"/> class.
        /// </summary>
        public SafeRandom() : this((int)TickCount.Now)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeRandom"/> class.
        /// </summary>
        /// <param name="seed">The seed.</param>
        public SafeRandom(int seed)
        {
            Reseed(seed);
        }

        /// <summary>
        /// Returns a nonnegative random number.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer greater than or equal to zero and less than <see cref="F:System.Int32.MaxValue"/>.
        /// </returns>
        public override int Next()
        {
            int ret;

            // To be equal to System.Random, we cannot return int.MaxValue
            do
            {
                ret = NextInt();
            }
            while (ret == 0x7FFFFFFF);

            return ret;
        }

        /// <summary>
        /// Returns a random number within a specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned.<paramref name="maxValue"/>
        /// must be greater than or equal to <paramref name="minValue"/>.</param>
        /// <returns>
        /// A 32-bit signed integer greater than or equal to <paramref name="minValue"/> and less than
        /// <paramref name="maxValue"/>; that is, the range of return values includes <paramref name="minValue"/>
        /// but not <paramref name="maxValue"/>. If <paramref name="minValue"/> equals <paramref name="maxValue"/>,
        /// <paramref name="minValue"/> is returned.
        /// </returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="minValue"/> is greater than
        /// <paramref name="maxValue"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="maxValue"/> is less than <paramref name="minValue"/>.</exception>
        public override int Next(int minValue, int maxValue)
        {
            if (minValue > maxValue)
                throw new ArgumentOutOfRangeException("maxValue", maxValue, "maxValue must be >= minValue");

            var range = maxValue - minValue;
            if (range < 0)
            {
                // If range is <0 then an overflow has occured and must resort to using long integer arithmetic instead (slower).
                // We also must use all 32 bits of precision, instead of the normal 31, which again is slower.	
                return minValue + (int)((_realUnitUInt * NextUInt()) * ((long)maxValue - minValue));
            }

            // 31 bits of precision will suffice if range <= int.MaxValue. This allows us to cast to an int and gain
            // a little more performance.
            return minValue + (int)((_realUnitInt * (int)(0x7FFFFFFF & NextUInt())) * range);
        }

        /// <summary>
        /// Returns a nonnegative random number less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated.
        /// <paramref name="maxValue"/> must be greater than or equal to zero.</param>
        /// <returns>
        /// A 32-bit signed integer greater than or equal to zero, and less than <paramref name="maxValue"/>; that is,
        /// the range of return values ordinarily includes zero but not <paramref name="maxValue"/>.
        /// However, if <paramref name="maxValue"/> equals zero, <paramref name="maxValue"/> is returned.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="maxValue"/> is less than zero.</exception>
        public override int Next(int maxValue)
        {
            if (maxValue < 0)
                throw new ArgumentOutOfRangeException("maxValue", maxValue, "maxValue must be greater than or equal to zero.");

            return (int)((_realUnitInt * NextInt()) * maxValue);
        }

        /// <summary>
        /// Returns a random boolean value.
        /// </summary>
        /// <returns>A random boolean value.</returns>
        public bool NextBool()
        {
            if (_bitMask == 1)
            {
                // Generate 32 more bits
                var t = (_x ^ (_x << 11));
                _x = _y;
                _y = _z;
                _z = _w;
                _bitBuffer = _w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8));

                // Reset the bitMask that tells us which bit to read next
                _bitMask = 0x80000000;
                return (_bitBuffer & _bitMask) == 0;
            }

            return (_bitBuffer & (_bitMask >>= 1)) == 0;
        }

        /// <summary>
        /// Fills the elements of a specified array of bytes with random numbers.
        /// </summary>
        /// <param name="buffer">An array of bytes to contain random numbers.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="buffer"/> is null. </exception>
        public override void NextBytes(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            var x = _x;
            var y = _y;
            var z = _z;
            var w = _w;

            var i = 0;
            uint t;

            // Generate 4 values at a time
            var bound = buffer.Length - 3;
            while (i < bound)
            {
                t = (x ^ (x << 11));
                x = y;
                y = z;
                z = w;
                w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));

                buffer[i++] = (byte)w;
                buffer[i++] = (byte)(w >> 8);
                buffer[i++] = (byte)(w >> 16);
                buffer[i++] = (byte)(w >> 24);
            }

            // Generate the remaining values
            if (i < buffer.Length)
            {
                t = (x ^ (x << 11));
                x = y;
                y = z;
                z = w;
                w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));

                buffer[i++] = (byte)w;
                if (i < buffer.Length)
                {
                    buffer[i++] = (byte)(w >> 8);
                    if (i < buffer.Length)
                    {
                        buffer[i++] = (byte)(w >> 16);
                        if (i < buffer.Length)
                            buffer[i] = (byte)(w >> 24);
                    }
                }
            }

            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }

        /// <summary>
        /// Returns a random number between 0.0 and 1.0.
        /// </summary>
        /// <returns>
        /// A double-precision floating point number greater than or equal to 0.0, and less than 1.0.
        /// </returns>
        public override double NextDouble()
        {
            return _realUnitInt * NextInt();
        }

        /// <summary>
        /// Generates a random number in the range of 0 to <see cref="int.MaxValue"/>, inclusive.
        /// </summary>
        /// <returns>A random number in the range of 0 to <see cref="int.MaxValue"/>, inclusive.</returns>
        int NextInt()
        {
            var t = (_x ^ (_x << 11));
            _x = _y;
            _y = _z;
            _z = _w;
            return (int)(0x7FFFFFFF & (_w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8))));
        }

        /// <summary>
        /// Generates an unsigned 32-bit number in the range of 0 to <see cref="uint.MaxValue"/>. This is the fastest
        /// method for generating random numbers.
        /// </summary>
        /// <returns>An unsigned 32-bit number in the range of 0 to <see cref="uint.MaxValue"/></returns>
        public uint NextUInt()
        {
            var t = (_x ^ (_x << 11));
            _x = _y;
            _y = _z;
            _z = _w;
            return (_w = (_w ^ (_w >> 19)) ^ (t ^ (t >> 8)));
        }

        /// <summary>
        /// Reinitializes the object using the specified seed value.
        /// </summary>
        /// <param name="seed">The seed.</param>
        public void Reseed(int seed)
        {
            // The only stipulation stated for the xorshift RNG is that at least one of
            // the seeds x,y,z,w is non-zero. We fulfill that requirement by only allowing
            // resetting of the x seed.
            _x = (uint)seed;
            _y = _cY;
            _z = _cZ;
            _w = _cW;
        }
    }
}